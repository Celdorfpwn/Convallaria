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
        private StreamSocket _socket { get; set; }
        IRequesterActions _actions { get; set; }

        IListenerAddress _address { get; set; }

        public StreamRequester()
        {
            _socket = new StreamSocket();
        }

        public void Request(IRequesterActions actions, IListenerAddress address)
        {
            bool connected = true;
            _actions = actions;
            _address = address;
            try
            {
                if (_socket.Information.RemoteAddress == null)
                {
                    _socket.ConnectAsync(new HostName(_address.Ip), _address.Port).AsTask().Wait();
                }
            }
            catch (Exception e)
            {
                connected = false;
                Debug.WriteLine(e.ToString());
            }

            if(connected)
            {
                Stream();
            }
        }

        private void Stream()
        {
            try
            {
                _socket.OutputStream.WriteMessage(_actions.Message);
                var dataReader = new DataReader(_socket.InputStream);
                IAsyncOperation<uint> stringHeader = dataReader.LoadAsync(4);
                stringHeader.AsTask().Wait();
                var strLength = dataReader.ReadUInt32();
                IAsyncOperation<uint> taskLoad = dataReader.LoadAsync(strLength);
                taskLoad.AsTask().Wait();
                uint numStrBytes = taskLoad.GetResults();
                string message = dataReader.ReadString(numStrBytes);
                _actions.ReadMessageResult(message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return;
            }

            Stream();
        }

    }
}
