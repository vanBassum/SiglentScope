namespace SCPI
{
    public interface IQuery
    {
        Status Status { get; set; }
        string Query { get; }
        bool Parse(byte[] data);
    }



}


