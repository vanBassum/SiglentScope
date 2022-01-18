using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using STDLib.Ethernet;

namespace Siglent
{
    class SCPI
    {
        TcpSocketClient client;

        public SCPI()
        {
            client = new TcpSocketClient();
            client.OnDataRecieved += Client_OnDataRecieved;
        }








        public async Task<bool> ConnectAsync(string host, CancellationTokenSource cts = null)
        {
            return await client.ConnectAsync(host, cts);
        }

        TaskCompletionSource<byte[]> recieved;
        List<byte> rxBuf = new List<byte>();
        private void Client_OnDataRecieved(object sender, byte[] e)
        {
            rxBuf.AddRange(e);
            if(rxBuf.Last() == 10)
            {
                if (recieved != null)
                    recieved.TrySetResult(rxBuf.ToArray());
                rxBuf.Clear();

            }
        }


        public async Task<string> SendCommand(string cmd, CancellationTokenSource cts = null)
        {
            return Encoding.ASCII.GetString(await SendCommand(Encoding.ASCII.GetBytes(cmd)));
        }


        public async Task<byte[]> SendCommand(byte[] cmd, CancellationTokenSource cts = null)
        {
            byte[] result = null;

            if (cts == null) (cts = new CancellationTokenSource()).CancelAfter(2500);
            recieved = new TaskCompletionSource<byte[]>();
            //cts.Token.Register(() => { recieved.TrySetCanceled(); });

            rxBuf.Clear();
            client.SendData(cmd);

            try
            {
                result = await recieved.Task;
            }
            catch (Exception exc)
            {

            }

            return result;
        }


        

    }





}

