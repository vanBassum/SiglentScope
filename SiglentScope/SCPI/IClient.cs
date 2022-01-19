namespace SCPI
{
    public interface IClient
    {
        void Connect(string host);
        Status ExecuteCommand(ICommand command);
        Status ExecuteQuery(IQuery command);
        
    }




}


