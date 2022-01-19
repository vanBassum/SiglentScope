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

        public decimal GetAmplitude()
        {
            PAVA cmd = new PAVA();
            cmd.Channel = Name;
            cmd.Parameters = new string[] { "AMPL" };
            if (Client.ExecuteCommand(cmd) == Status.Success)
            {
                return cmd.Value;
            }
            return 0;
        }

        public decimal GetFrequency()
        {
            PAVA cmd = new PAVA();
            cmd.Channel = Name;
            cmd.Parameters = new string[] { "FREQ" };
            if (Client.ExecuteCommand(cmd) == Status.Success)
            {
                return cmd.Value;
            }
            return 0;
        }

        public decimal GetPhase(Channel channel)
        {
            throw new NotImplementedException();
        }



    }


}


