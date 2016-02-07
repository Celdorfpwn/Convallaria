using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.Components;
using ThingSockets.External;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ThingSockets
{
    public sealed class StreamRequester : IRequester
    {
        private ComplexStreamSocket _socket { get; set; }
        IRequesterActions _actions { get; set; }

        IListenerAddress _address { get; set; }

        public StreamRequester()
        {
            _socket = new ComplexStreamSocket(new StreamSocket());
        }

        public void Request(IRequesterActions actions, IListenerAddress address)
        {
            bool connected = true;
            _actions = actions;
            _address = address;
            try
            {
                if (_socket.Socket.Information.RemoteAddress == null)
                {
                    _socket.Socket.ConnectAsync(new HostName(_address.Ip), _address.Port).AsTask().Wait();
                }
            }
            catch (Exception e)
            {
                connected = false;
                Debug.WriteLine(e.ToString());
            }

            if(connected)
            {
                while(Stream());
            }
        }

        private bool Stream()
        {
            var continueStreaming = true;
            try
            {
                _socket.DataWriter.WriteMessage(_actions.Message);
                string message = _socket.DataReader.GetMessage();
                _actions.ReadMessageResult(message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                continueStreaming = false;
            }
            return continueStreaming;
        }

    }
}
