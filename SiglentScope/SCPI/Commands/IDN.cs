﻿using System.Linq;
using System.Text;

namespace SCPI.Commands
{
    //https://github.com/tparviainen/oscilloscope/blob/master/SCPI/IEEE4882/IDN.cs
    public class IDN : ICommand
    {
        public Status Status { get; set; }
        public string Description => "Query the ID string of the instrument";
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public string SerialNumber { get; private set; }
        public string SoftwareVersion { get; private set; }
        public string Command => "*IDN?";

        public bool Parse(byte[] data)
        {
            var id = Encoding.ASCII.GetString(data).Split(',').Select(f => f.Trim());
            // According to IEEE 488.2 there are four fields in the response
            if (id.Count() == 4)
            {
                Manufacturer = id.ElementAt(0);
                Model = id.ElementAt(1);
                SerialNumber = id.ElementAt(2);
                SoftwareVersion = id.ElementAt(3);
                return true;
            }
            return false;
        }
    }
}


