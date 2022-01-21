using SCPI.Devices.SDS1104XE.Commands;
using System;
using System.Collections.Generic;

namespace SCPI.Devices.SDS1104XE
{
    public class Channel
    {
        public string Name { get; }
        public IClient Client { get; }

        public Channel(string name, IClient client)
        {
            Name = name;
            Client = client;
        }

        public decimal? Amplitude => GetPava("AMPL");
        public decimal? Frequency => GetPava("FREQ");

        public decimal? VDIV 
        { 
            get 
            {
                VDIV cmd = new VDIV();
                cmd.Channel = Name;
                if (Client.ExecuteQuery(cmd) == Status.Success)
                    return cmd.Value;
                return null;
            } 
            set 
            {
                VDIV cmd = new VDIV();
                cmd.Value = value.Value;
                cmd.Channel = Name;
                if (Client.ExecuteCommand(cmd) != Status.Success)
                    throw cmd.Status.Exception;
            } 
        }


        public decimal? Offset
        {
            get
            {
                OFST cmd = new OFST();
                cmd.Channel = Name;
                if (Client.ExecuteQuery(cmd) == Status.Success)
                    return cmd.Value;
                return null;
            }
            set
            {
                OFST cmd = new OFST();
                cmd.Value = value.Value;
                cmd.Channel = Name;
                if (Client.ExecuteCommand(cmd) != Status.Success)
                    throw cmd.Status.Exception;
            }
        }


        public IEnumerable<decimal> GetWaveform(int index, int count, int sparsing)
        {
            decimal vdiv = VDIV??0;
            decimal ofst = Offset??0;
            SetupWaveform(index, count, sparsing);
            byte[] rawWaveform = GetRawWaveform();

            for (int i = 0; i < rawWaveform.Length; i++)
            {
                decimal x = rawWaveform[i];
                if (x > 127)
                    x -= 255;
                yield return x * (vdiv / 25m) - ofst;
            }
        }


        private void SetupWaveform(int index, int count, int sparsing)
        {
            WFSU cmd = new WFSU();
            cmd.FirstPoint = index;
            cmd.SampleSize = count;
            cmd.Sparsing = sparsing;
            cmd.Channel = Name;
            Client.ExecuteCommand(cmd);
        }


        private byte[] GetRawWaveform()
        {
            WF cmd = new WF();
            cmd.Channel = Name;
            Client.ExecuteQuery(cmd);
            return cmd.Data;
        }



        public decimal GetPhase(Channel channel)
        {
            throw new NotImplementedException();
        }



        private decimal? GetPava(string param)
        {
            PAVA cmd = new PAVA();
            cmd.Channel = Name;
            cmd.Parameters = new string[] { param };
            if (Client.ExecuteQuery(cmd) == Status.Success)
                return cmd.Value;
            return null;
        }
    }
}


