using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FRMLib.Scope;
using Siglent;
using STDLib.Ethernet;

namespace SiglentScope
{
    public partial class Form1 : Form
    {
        SDS1104 siglent = new SDS1104();
        ScopeController scope = new ScopeController();

        Trace t1 = new Trace { };


        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            scopeView1.DataSource = scope;
            scope.Traces.Add(t1);
            timer1.Start();
            timer1.Interval = 1000;
            trackBar1.Value = trackBar1.Maximum / 4;
            await siglent.ConnectAsync(textBox2.Text);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
        }



        private async void button2_Click(object sender, EventArgs e)
        {

        }
        double[] pts1;
        double[] pts2;

        object lck = new object();

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            draw();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                pts1 = await siglent.GetWaveForm(SDS1104.Channels.C1);
                pts2 = await siglent.GetWaveForm(SDS1104.Channels.C3);
                draw();
            }
            catch { }
            timer1.Start();
        }

        void draw()
        {
            if (pts1 != null)
            {

                t1.Points.Clear();
                double offset = -1E-6 * (trackBar1.Value - trackBar1.Maximum / 2);

                double p = 0;
                for (int i = 0; i < pts1.Length; i++)
                {
                    p += -pts1[i] + offset;

                    t1.Points.Add(pts2[i], p);
                }

                t1.DrawStyle = Trace.DrawStyles.Lines;
                t1.Scale = 0.5;
                t1.Offset = -2;
                scopeView1.AutoScaleHorizontal();
            }
        }

    }


    



}

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


    class SCPI
    {
        TcpSocketClient client;

        public SCPI()
        {
            client = new TcpSocketClient();
            client.OnDataRecieved += Client_OnDataRecieved;
        }








        public async Task<bool> ConnectAsync(string host, CancellationTokenSource cts = null)
        {
            return await client.ConnectAsync(host, cts);
        }

        TaskCompletionSource<byte[]> recieved;
        List<byte> rxBuf = new List<byte>();
        private void Client_OnDataRecieved(object sender, byte[] e)
        {
            rxBuf.AddRange(e);
            if(rxBuf.Last() == 10)
            {
                if (recieved != null)
                    recieved.TrySetResult(rxBuf.ToArray());
                rxBuf.Clear();

            }
        }


        public async Task<string> SendCommand(string cmd, CancellationTokenSource cts = null)
        {
            return Encoding.ASCII.GetString(await SendCommand(Encoding.ASCII.GetBytes(cmd)));
        }


        public async Task<byte[]> SendCommand(byte[] cmd, CancellationTokenSource cts = null)
        {
            byte[] result = null;

            if (cts == null) (cts = new CancellationTokenSource()).CancelAfter(2500);
            recieved = new TaskCompletionSource<byte[]>();
            //cts.Token.Register(() => { recieved.TrySetCanceled(); });

            rxBuf.Clear();
            client.SendData(cmd);

            try
            {
                result = await recieved.Task;
            }
            catch (Exception exc)
            {

            }

            return result;
        }


        

    }





}

