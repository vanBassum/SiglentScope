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


        SDS1104XE siglent;


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

            TCPSCPI client = new TCPSCPI();
            //client.Connect(textBox2.Text);
            IDN idn = new IDN();
            if (client.ExecuteCommand(idn) == Status.Success)
            {
                //Bla bla if idn == SDS1104XE

                siglent = new SDS1104XE(client);
                timer1.Start();
            }
        }


        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                decimal aplitude1 = siglent.Channels[0].GetAmplitude();

                scopeView1.Refresh();

            }
            catch { }
            timer1.Start();
        }

    }
}
