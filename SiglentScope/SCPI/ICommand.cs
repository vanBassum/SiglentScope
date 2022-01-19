namespace SCPI
{
    //https://github.com/tparviainen/oscilloscope/blob/master/SCPI/ICommand.cs
    public interface ICommand
    {
        Status Status { get; set; }
        string Command { get; }
    }



}


