namespace SCPI.Devices.SDS1104XE.Commands
{
    public class WFSU : ICommand//, IQuery
    {
        public Status Status { get; set; }
        public string Channel { get; set; } = "C1";
        public decimal FirstPoint { get; set; } = 0;
        public decimal Sparsing { get; set; } = 0;
        public decimal SampleSize { get; set; } = 100;


        public string Command => $"WFSU SP, {Sparsing}, NP, {SampleSize}, FP, {FirstPoint}";
        //public string Query => $"{Channel}:VDIV?";
        
        //public bool Parse(byte[] data)
        //{
        //    var str = Encoding.ASCII.GetString(data).Trim('\0', '\n').Split(' ').Last();
        //    if (Convert.ToDecimal(str, out decimal value))
        //    {
        //        Value = value;
        //        return true;
        //    }
        //    return false;
        //}
    }
}


