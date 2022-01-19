using STDLib.Ethernet;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SCPI.Clients
{
    //https://github.com/sulmar/ScpiDemo/blob/master/ScpiDemo/ScpiClient.cs
    public class TCPSCPI : IClient, IDisposable
    {
        Stopwatch sw = new Stopwatch();
        private TcpClient Client { get; } = new TcpClient();

        Stream stream;

        ~TCPSCPI()
        {
            Dispose();
        }

        public void Dispose()
        {
            Client.Close();
        }

        public void Connect(string host)
        {
            IPEndPoint ep = DNSExt.Parse(host);
            Client.Connect(ep);
            stream = Client.GetStream();

            byte[] buf = new byte[1024];
            stream.Read(buf, 0, buf.Length);
        }





        public Status ExecuteCommand(ICommand command)
        {
            Status result = Status.Unknown;

            if (Client.Connected)
            {
                byte[] response;
                try
                {
                    response = Send(command.Command, false);   //TODO: What about errorhandling?
                    result = Status.Success;
                }
                catch (Exception exception)
                {
                    result = Status.SendingError;
                    result.Exception = exception;
                }
            }
            else
                result = Status.NotConnected;

            result.Duration = sw.Elapsed;
            command.Status = result;
            return result;
        }

        public Status ExecuteQuery(IQuery command)
        {
            Status result = Status.Unknown;

            if (Client.Connected)
            {
                byte[] response;
                try
                {
                    response = Send(command.Query, true);
                    if (command.Parse(response))
                    {
                        result = Status.Success;
                    }
                    else
                        result = Status.ParsingError;
                }
                catch (Exception exception)
                {
                    result = Status.SendingError;
                    result.Exception = exception;
                }
            }
            else
                result = Status.NotConnected;

            result.Duration = sw.Elapsed;
            command.Status = result;
            return result;
        }


        private byte[] Send(string request, bool waitForEOL)
        {
            sw.Restart();
            byte[] response = new byte[1024];
            var data = Encoding.ASCII.GetBytes(request + "\n");
            stream.Write(data, 0, data.Length);
            int rdPtr = 0;
            do
            {
                rdPtr += stream.Read(response, rdPtr, response.Length - rdPtr);
            }
            while (!response.Contains((byte)'\n') && waitForEOL);
            Array.Resize(ref response, rdPtr+1);
            sw.Stop();
            return response;
        }
    }
}


