using SCPI.Devices.SDS1104XE.Commands;

namespace SCPI.Devices.SDS1104XE
{
    public class SDS1104XE
    {
        public IClient Client { get; }
        public Channel[] Channels { get; }

        public SDS1104XE(IClient client)
        {
            Client = client;
            Channels = new Channel[] 
            { 
                new Channel("C1", Client),
                new Channel("C2", Client),
                new Channel("C3", Client),
                new Channel("C4", Client),
            };
        }


        public decimal? TDIV
        {
            get
            {
                TDIV cmd = new TDIV();
                if (Client.ExecuteQuery(cmd) == Status.Success)
                    return cmd.Value;
                return null;
            }
            set
            {
                TDIV cmd = new TDIV();
                cmd.Value = value.Value;
                if (Client.ExecuteCommand(cmd) != Status.Success)
                    throw cmd.Status.Exception;
            }
        }
    }


}


