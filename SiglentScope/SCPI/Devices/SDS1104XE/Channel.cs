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

        public decimal? GetAmplitude() => Pava("AMPL");
        public decimal? GetFrequency() => Pava("FREQ");



        public decimal GetPhase(Channel channel)
        {
            throw new NotImplementedException();
        }



        private decimal? Pava(string param)
        {
            PAVA cmd = new PAVA();
            cmd.Channel = Name;
            cmd.Parameters = new string[] { param };
            if (Client.ExecuteCommand(cmd) == Status.Success)
                return cmd.Value;
            return null;
        }
    }
}


