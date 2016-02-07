using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.Components;
using ThingSockets.External;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ThingSockets
{
    public sealed class Listener : IListener
    {
        public bool HasConnection { get; private set; }

        private StreamSocketListener _listener { get; set; }

        public string Port { get; private set; }

        private IListenerActions _actions { get; set; }

        public Listener()
        {
            _listener = new StreamSocketListener();
            _listener.Control.KeepAlive = false;
            _listener.ConnectionReceived += ConnectionReceived;
        }
        public void Start(int port, IListenerActions actions)
        {
            Port = port.ToString();
            _actions = actions;
            _listener.BindServiceNameAsync(Port).AsTask().Wait();
        }

        private void ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            HasConnection = true;
            try
            {
                var message = _actions.Response(new DataReader(args.Socket.InputStream).GetMessage());
                new DataWriter(args.Socket.OutputStream).WriteMessage(message);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(SocketError.GetStatus(exception.HResult));
            }
            HasConnection = false;
        }


    }
}
