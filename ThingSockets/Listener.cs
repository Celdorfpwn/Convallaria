using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ThingSockets
{
    sealed class Listener : IListener
    {
        private const uint BufferSize = 8192;
        private StreamSocketListener _listener { get; set; }

        private List<StreamSocket> _connections { get; set; }

        private IListenerIOActions _socketIOActions { get; set; }

        public Listener(IListenerIOActions socketIOActions)
        {
            _socketIOActions = socketIOActions;
            _connections = new List<StreamSocket>();
            _listener = new StreamSocketListener();
            _listener.ConnectionReceived += ConnectionReceived;
        }

        public void Start(int port)
        {
            _listener.BindServiceNameAsync(port.ToString()).AsTask().Wait();
        }

        private void ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            _connections.Add(args.Socket);
            ReadInput(args.Socket);
            WriteOutput(args.Socket);
        }

        private void ReadInput(StreamSocket socket)
        {
            using (var dataReader = new DataReader(socket.InputStream))
            {
                dataReader.LoadAsync(4).AsTask().Wait();
                var lenght = dataReader.ReadUInt32();
                IAsyncOperation<uint> taskLoad = dataReader.LoadAsync(lenght);
                taskLoad.AsTask().Wait();
                uint bytesNumber = taskLoad.GetResults();
                string message = dataReader.ReadString(bytesNumber);
                _socketIOActions.ReadInput(message);
            }
        }


        async private void WriteOutput(StreamSocket socket)
        {
            using (var writer = new DataWriter(socket.OutputStream))
            {
                var message = _socketIOActions.GetOutput();
                var length = writer.MeasureString(message);
                writer.WriteInt32((int)length);
                writer.WriteString(message);
                var ret = await writer.StoreAsync();
                writer.DetachStream();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            WriteOutput(socket);
        }
    }
}
