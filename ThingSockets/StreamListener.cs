using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.Components;
using ThingSockets.External;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ThingSockets
{
    public sealed class StreamListener : IListener
    {
        private StreamSocketListener _listener { get; set; }

        private string _port { get; set; }

        private IListenerActions _actions { get; set; }

        public StreamListener()
        {
            _listener = new StreamSocketListener();
            _listener.Control.KeepAlive = false;
            _listener.ConnectionReceived += ConnectionReceived;
        }

        public void Start(int port,IListenerActions actions)
        {
            _port = port.ToString();
            _actions = actions;
            _listener.BindServiceNameAsync(_port).AsTask().Wait();
        }

        private void ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {           
            Stream(args.Socket);
        }



        private void Stream(StreamSocket socket)
        {
            bool continueStreaming = true; 
            try
            {
                var message = _actions.Response(socket.InputStream.GetMessage());
                continueStreaming = socket.OutputStream.WriteMessage(message);
            }
            catch(Exception exception)
            {
                continueStreaming = false;
                Debug.WriteLine(SocketError.GetStatus(exception.HResult));
            }

            if (continueStreaming)
            {
                Stream(socket);
            }
        }
    }
}
