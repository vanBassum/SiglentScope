using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FRMLib.Scope;
using SCPI;
using SCPI.Commands;
using SCPI.Clients;
using SCPI.Devices.SDS1104XE;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace SiglentScope
{
    public partial class Form1 : Form
    {
        ScopeController scope = new ScopeController();
        Trace amplitude = new Trace { Pen = new Pen(Color.Yellow), DrawStyle = Trace.DrawStyles.Lines, Scale = 0.125f, Offset = -0.5f };
        Trace phase = new Trace { Pen = new Pen(Color.Red) };

        SDS1104XE siglent = new SDS1104XE(new TCPSCPI());


        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            scopeView1.DataSource = scope;
            scope.Traces.Add(amplitude);
            scope.Traces.Add(phase);
            timer1.Interval = 100;
            siglent.Client.Connect(textBox2.Text);
            timer1.Start();

            



            //siglent.Channels[0].VDIV = vdiv1;
            //siglent.Channels[2].VDIV = vdiv2;

            //siglent.TDIV = (decimal?)100E-6;

            //IDN idn = new IDN();
            //if (client.ExecuteCommand(idn) == Status.Success)
            //{
            //    //Bla bla if idn == SDS1104XE
            //
            //    siglent = new SDS1104XE(client);
            //    timer1.Start();
            //}
        }

        Stopwatch sw = new Stopwatch();
        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            int i = 0;
            int pts = 140000;
            int cnt = 500;
            int sparse = pts / cnt;
            sw.Restart();
            var result = siglent.Channels[0].GetWaveform(0, cnt, sparse);
            amplitude.Points.Clear();
            foreach (var sample in result)
                amplitude.Points.Add(new STDLib.Math.PointD(i++, (double)sample));
            sw.Stop();
            this.Text = sw.ElapsedMilliseconds.ToString();
            amplitude.Offset = (double)siglent.Channels[0].Offset;
            amplitude.Scale = (double)siglent.Channels[0].VDIV;

            scopeView1.AutoScaleHorizontal();
            scopeView1.Refresh();
            timer1.Start();
        }

        decimal vdiv1 = 1;
        decimal vdiv2 = 1;


        void AdjustVDIV(int ch, decimal ampl)
        {
            decimal prev = ch == 0 ? vdiv1 : vdiv2;
            decimal next = prev;
            decimal max = prev * 4;
            decimal min = prev * 4;
            max -= max / 8;
            min -= min / 4;

            if (ampl > max*2 || ampl < min*2)
                next = ampl / 7;


            if (ch == 0)
            {
                if (next != prev)
                    siglent.Channels[0].VDIV = next;
                vdiv1 = next;
            }
                
            else
            {
                if (next != prev)
                    siglent.Channels[2].VDIV = next;
                vdiv2 = next;
            }
        }

    }
}
