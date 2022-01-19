using System.Linq;
using System.Text;

namespace SCPI.Devices.SDS1104XE.Commands
{
    public class OFST : ICommand, IQuery
    {
        public Status Status { get; set; }
        public string Channel { get; set; } = "C1";
        public string Command => $"{Channel}:OFST {Value.ToString("E5")}";
        public string Query => $"{Channel}:OFST?";
        public decimal Value { get; set; }
        public bool Parse(byte[] data)
        {
            var str = Encoding.ASCII.GetString(data).Trim('\0', '\n').Split(' ').Last();
            if (Convert.ToDecimal(str, out decimal value))
            {
                Value = value;
                return true;
            }
            return false;
        }
    }
}


