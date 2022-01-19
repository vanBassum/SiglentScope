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

namespace SiglentScope
{
    public partial class Form1 : Form
    {
        ScopeController scope = new ScopeController();
        Trace gain = new Trace { Pen = new Pen(Color.Yellow), DrawStyle = Trace.DrawStyles.Points, Scale = 0.125f, Offset = -0.5f };
        Trace phase = new Trace { Pen = new Pen(Color.Red) };

        SDS1104XE siglent = new SDS1104XE(new TCPSCPI());


        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            scopeView1.DataSource = scope;
            scope.Traces.Add(gain);
            scope.Traces.Add(phase);
            timer1.Interval = 100;
            siglent.Client.Connect(textBox2.Text);
            timer1.Start();

            siglent.Channels[0].VDIV = vdiv1;
            siglent.Channels[2].VDIV = vdiv2;

            siglent.TDIV = (decimal?)100E-6;

            //IDN idn = new IDN();
            //if (client.ExecuteCommand(idn) == Status.Success)
            //{
            //    //Bla bla if idn == SDS1104XE
            //
            //    siglent = new SDS1104XE(client);
            //    timer1.Start();
            //}
        }


        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            decimal aplitude1 = siglent.Channels[0].Amplitude ?? 0;
            decimal aplitude2 = siglent.Channels[2].Amplitude ?? 0;
            decimal freq = siglent.Channels[0].Frequency ?? 0;

            decimal vdiv = siglent.Channels[0].VDIV ?? 0;

            double a1 = (double)aplitude1;
            double a2 = (double)aplitude2;
            double f1 = (double)freq;


            //AdjustVDIV(0, aplitude1);
            //AdjustVDIV(1, aplitude2);



            gain.Points.Add(new STDLib.Math.PointD(f1, a2 / a1));


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
