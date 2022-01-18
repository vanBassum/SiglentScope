using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Siglent
{
    class SDS1104
    {
        SCPI scpi = new SCPI();

        public SDS1104()
        {

        }

        public async Task<bool> ConnectAsync(string host, CancellationTokenSource cts = null)
        {
            return await scpi.ConnectAsync(host, cts);
        }

        public async Task SetupWaveForm(int sp = 1, int np = 0, int fp = 0, CancellationTokenSource cts = null)
        {
            await scpi.SendCommand(Encoding.ASCII.GetBytes($"WFSU SP, {sp}, NP, {np}, FP, {fp}"), cts);
        }


        public async Task<double> Measure_Amplitude(Channels channel, CancellationTokenSource cts = null)
        {
            double amplitude = await GetDouble($"{channel}:PAVA? AMPL", cts);
            return amplitude;
        }

        public async Task<double> Measure_Frequention(Channels channel, CancellationTokenSource cts = null)
        {
            double freq = await GetDouble($"{channel}:PAVA? FREQ", cts);
            return freq;
        }

        public async Task<double> Measure_Phase(Channels channel1, Channels channel2, CancellationTokenSource cts = null)
        {
            double phase = await GetDouble($"{channel1}-{channel2}:MEAD? PHA", cts);
            return phase;
        }



        public async Task<double[]> GetWaveForm(Channels channel, CancellationTokenSource cts = null)
        {
            double vdiv = await GetVDIV(channel, cts);
            double ofst = await GetOFST(channel, cts);

            //string description = await scpi.SendCommand($"{channel}:WaveForm? DESC", cts);

            byte[] wave = await scpi.SendCommand(Encoding.ASCII.GetBytes($"{channel}:WaveForm? DAT2"), cts);

            string header = Encoding.ASCII.GetString(wave,0,  25);
            Match blkLen = Regex.Match(header, @"#.(\d+)");


            int blkSize = int.Parse(blkLen.Groups[1].Value);
            int start = blkLen.Index + blkLen.Length;

            double[] res = new double[blkSize];

            for (int i = 0; i < blkSize; i++)
            {
                double x = wave[i + start];
                if (x > 127)
                    x -= 255;

                res[i] = x * (vdiv / 25) - ofst;
            }

            return res;
        }





        public async Task<double> GetVDIV(Channels channel, CancellationTokenSource cts = null)
        {
            return await GetDouble($"{channel}:VDIV?", cts);
        }


        public async Task<double> GetOFST(Channels channel, CancellationTokenSource cts = null)
        {
            return await GetDouble($"{channel}:OFST?", cts);
        }


        







        public async Task<double> GetDouble(string cmd, CancellationTokenSource cts = null)
        {
            string res = await scpi.SendCommand(cmd, cts);
            int ind = res.IndexOf(',');

            if(ind != -1)
            {
                string sub = res.Substring(ind+1);
                Match ma = Regex.Match(sub, @"[-+\dE.]+");
                if (double.TryParse(ma.Value, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out double result))
                    return result;
            }
            Match m = Regex.Match(res, @"(-?\d+)\.?\d+(E-|E\+|E|\d+)\d+");
            return double.Parse(m.Value, NumberStyles.Number | NumberStyles.AllowExponent);
        }
        



        public enum Channels
        { 
            C1,
            C2,
            C3,
            C4
        }


    }





}

