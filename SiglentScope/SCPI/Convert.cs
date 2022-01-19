using System.Globalization;
using System.Text.RegularExpressions;

namespace SCPI
{
    public static class Convert
    {
        public static bool ToDecimal(string str, out decimal value)
        {
            Match ma = Regex.Match(str, @"[-+\dE.]+");
            if(ma.Success)
            {
                if (decimal.TryParse(ma.Value, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out decimal result))
                {
                    value = result;
                    return true;
                }
            }
            value = 0;
            return false;
        }
    }
}


