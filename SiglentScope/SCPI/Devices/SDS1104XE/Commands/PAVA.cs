namespace SCPI.Devices.SDS1104XE.Commands
{
    public class PAVA : ICommand
    {
        public Status Status { get; set; }
        public string Channel { get; set; } = "C1";
        public string[] Parameters { get; set; }
        public string Command => $"{Channel}:PAVA? {string.Join(", ", Parameters)}";
        public decimal Value { get; }
        public bool Parse(byte[] data)
        {
            throw new System.NotImplementedException();
        }
    }
}


