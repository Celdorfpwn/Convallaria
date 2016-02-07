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
        public bool HasConnection { get; private set; }
        private StreamSocketListener _listener { get; set; }

        public string Port { get; private set; }

        private IListenerActions _actions { get; set; }


        public StreamListener()
        {
            _listener = new StreamSocketListener();
            _listener.ConnectionReceived += ConnectionReceived;
        }

        public void Start(int port,IListenerActions actions)
        {
            Port = port.ToString();
            _actions = actions;
            _listener.BindServiceNameAsync(Port).AsTask().Wait();
        }

        private void ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            HasConnection = true;
            var socket = new ComplexStreamSocket(args.Socket);
            StartStreaming(socket);
            HasConnection = false;
        }

        void StartStreaming(ComplexStreamSocket socket)
        {
            while (Stream(socket));
        }


        private bool Stream(ComplexStreamSocket socket)
        {
            bool continueStreaming = true;
            try
            {
                var message = socket.DataReader.GetMessage();
                var result = _actions.Response(message);
                Debug.WriteLine("Sending " + message);
                continueStreaming = socket.DataWriter.WriteMessage(result);
            }
            catch (Exception exception)
            {
                continueStreaming = false;
                Debug.WriteLine(SocketError.GetStatus(exception.HResult));
            }
            return continueStreaming;
        }
    }
}
