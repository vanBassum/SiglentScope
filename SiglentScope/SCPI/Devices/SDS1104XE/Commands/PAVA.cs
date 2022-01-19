using System;
using System.Linq;
using System.Text;

namespace SCPI.Devices.SDS1104XE.Commands
{
    public class PAVA : ICommand
    {
        public Status Status { get; set; }
        public string Channel { get; set; } = "C1";
        public string[] Parameters { get; set; }
        public string Command => $"{Channel}:PAVA? {string.Join(", ", Parameters)}";
        public decimal Value { get; private set; }
        public bool Parse(byte[] data)
        {
            //C1:PAVA AMPL,1.44E+00V
            var str = Encoding.ASCII.GetString(data).Trim('\0', '\n').Split(',').Last();
            if (Convert.ToDecimal(str, out decimal value))
            {
                Value = value;
                return true;
            }
            return false;
        }
    }
}


