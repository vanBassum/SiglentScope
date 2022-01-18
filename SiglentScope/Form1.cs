using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FRMLib.Scope;
using Siglent;

namespace SiglentScope
{
    public partial class Form1 : Form
    {
        SDS1104 siglent = new SDS1104();
        ScopeController scope = new ScopeController();
        Trace gain = new Trace { Pen = new Pen(Color.Yellow), DrawStyle = Trace.DrawStyles.Points, Scale = 0.125f, Offset = -0.5f };
        Trace phase = new Trace { Pen = new Pen(Color.Red) };

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            scopeView1.DataSource = scope;
            scope.Traces.Add(gain);
            scope.Traces.Add(phase);
            timer1.Start();
            timer1.Interval = 100;
            await siglent.ConnectAsync(textBox2.Text);
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                double freq = await siglent.Measure_Frequention(SDS1104.Channels.C1);
                double amplitude1 = await siglent.Measure_Amplitude(SDS1104.Channels.C1);
                double amplitude2 = await siglent.Measure_Amplitude(SDS1104.Channels.C3);
                //double phase = await siglent.Measure_Phase(SDS1104.Channels.C1, SDS1104.Channels.C3);


                gain.Points.Add(freq, amplitude2 / amplitude1);

                scopeView1.Refresh();

            }
            catch { }
            timer1.Start();
        }

    }
}
