﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.Components;
using ThingSockets.External;
using Windows.Networking.Sockets;

namespace ThingSockets
{
    public sealed class Listener : IListener
    {
        private StreamSocketListener _listener { get; set; }

        private string _port { get; set; }

        private IListenerActions _actions { get; set; }

        public Listener()
        {
            _listener = new StreamSocketListener();
            _listener.Control.KeepAlive = false;
            _listener.ConnectionReceived += ConnectionReceived;
        }
        public void Start(int port, IListenerActions actions)
        {
            _port = port.ToString();
            _actions = actions;
            _listener.BindServiceNameAsync(_port).AsTask().Wait();
        }

        private void ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            try
            {
                var message = _actions.Response(args.Socket.InputStream.GetMessage());
                args.Socket.OutputStream.WriteMessage(message);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(SocketError.GetStatus(exception.HResult));
            }
        }


    }
}
