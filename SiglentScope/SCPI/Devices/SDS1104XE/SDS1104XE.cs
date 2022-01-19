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
    }


}


