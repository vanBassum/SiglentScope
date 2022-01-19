using STDLib.Ethernet;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SCPI.Clients
{
    //https://github.com/sulmar/ScpiDemo/blob/master/ScpiDemo/ScpiClient.cs
    public class TCPSCPI : IClient, IDisposable
    {
        private TcpClient Client { get; } = new TcpClient();


        public void Connect(string host)
        {
            IPEndPoint ep = DNSExt.Parse(host);
            Client.Connect(ep);
        }

        public Status ExecuteCommand(ICommand command)
        {
            Status result = Status.Unknown;

            if (Client.Connected)
            {
                byte[] response;
                try
                {
                    response = Send(command.Command);

                    if (command.Parse(response))
                    {
                        result = Status.Success;
                    }
                    else
                        result = Status.ParsingError;

                }
                catch (Exception exception)
                {
                    result = new Status(Status.SendingError, exception.Message);
                }
            }
            else
                result = Status.NotConnected;

            command.Status = result;
            return result;
        }


        private byte[] Send(string request)
        {
            byte[] response = new byte[1024];
            var data = Encoding.ASCII.GetBytes(request);
            using (var stream = Client.GetStream())
            {
                stream.Write(data, 0, data.Length);
                int bytesRead = stream.Read(response, 0, response.Length);
                Array.Resize(ref response, bytesRead);
            }
            return response;
        }

        public void Dispose()
        {
            Client.Close();
        }

        

    }




}


