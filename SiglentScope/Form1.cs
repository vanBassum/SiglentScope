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
using Siglent;
using STDLib.Ethernet;

namespace SiglentScope
{
    public partial class Form1 : Form
    {
        SDS1104 siglent = new SDS1104();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await siglent.ConnectAsync(textBox2.Text);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await siglent.GetWaveForm(SDS1104.Channels.C3);
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



        public async Task<double[]> GetWaveForm(Channels channel, CancellationTokenSource cts = null)
        {
            double vdiv = await GetVDIV(channel, cts);
            double ofst = await GetOFST(channel, cts);

            //string description = await scpi.SendCommand($"{channel}:WaveForm? DESC", cts);

            byte[] wave = await scpi.SendCommand(Encoding.ASCII.GetBytes($"{channel}:WaveForm? DAT2"), cts);

            //Match blkLenM = Regex.Match(description, @"#.(\d+)");

            //int blkLen = int.Parse(blkLenM.Groups[1].Value);

            double[] res = new double[wave.Length];

            for (int i = 0; i < wave.Length; i++)
            {
                double x = wave[i];
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

