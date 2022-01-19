using SCPI.Devices.SDS1104XE.Commands;
using System;

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


