using System;
using System.Linq;
using System.Text;

namespace SCPI.Devices.SDS1104XE.Commands
{
    public class WF : IQuery
    {
        public Status Status { get; set; }
        public string Channel { get; set; } = "C1";





        public string Query => $"{Channel}:WF?";
        public bool Parse(byte[] data)
        {
            throw new NotImplementedException();
            var str = Encoding.ASCII.GetString(data).Trim('\0', '\n').Split(' ').Last();
            if (Convert.ToDecimal(str, out decimal value))
            {
                //Value = value;
                return true;
            }
            return false;
        }
    }
}


