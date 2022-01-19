namespace SCPI
{
    //https://github.com/tparviainen/oscilloscope/blob/master/SCPI/ICommand.cs
    public interface ICommand
    {
        Status Status { get; set; }
        /// <summary>
        /// Creates the SCPI command based on the input data
        /// </summary>
        /// <param name="parameters">List of parameters that is used to form the command</param>
        /// <returns>SCPI command</returns>
        string Command { get; }

        /// <summary>
        /// Parses and validates the data received from the instrument
        /// </summary>
        /// <param name="data">Received data</param>
        /// <returns>True if parsing succeeded and data is valid, otherwise false</returns>
        bool Parse(byte[] data);
    }


}


