using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.Components;
using ThingSockets.External;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ThingSockets
{
    public sealed class Requester : IRequester
    {


        public void Request(IRequesterActions actions, IListenerAddress address)
        {
            bool connected = true;
            StreamSocket socket = new StreamSocket();
            try
            {
                if (socket.Information.RemoteAddress == null)
                {
                    socket.ConnectAsync(new HostName(address.Ip), address.Port).AsTask().Wait();
                }
            }
            catch (Exception e)
            {
                connected = false;
                Debug.WriteLine(e.ToString());
            }

            if (connected)
            {
                Send(actions,socket);
            }
        }

        private void Send(IRequesterActions actions, StreamSocket socket)
        {
            try
            {
                new DataWriter(socket.OutputStream).WriteMessage(actions.Message);
                var message = new DataReader(socket.InputStream).GetMessage();
                actions.ReadMessageResult(message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return;
            }
        }
    }
}
