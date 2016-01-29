using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ThingSockets
{
    public sealed class Requester : IRequester
    {
        private StreamSocket _socket { get; set; }
        IRequesterReader _requesterReader { get; set; }

        IRequesterInfo _requesterInfo { get; set; }


        public Requester(IRequesterInfo requesterInfo, IRequesterReader requesterReader)
        {
            _requesterInfo = requesterInfo;
            _requesterReader = requesterReader;
            _socket = new StreamSocket();
        }

        async public void Request()
        {
            try
            {
                if (_socket.Information.RemoteAddress == null)
                {
                    await _socket.ConnectAsync(new HostName(_requesterInfo.Ip), _requesterInfo.Port);
                    WriteMessage(_requesterReader.GetMessage());
                    ReadInput();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private void ReadInput()
        {
            try
            {
                var dataReader = new DataReader(_socket.InputStream);
                IAsyncOperation<uint> stringHeader = dataReader.LoadAsync(4);
                stringHeader.AsTask().Wait();
                var strLength = dataReader.ReadUInt32();
                IAsyncOperation<uint> taskLoad = dataReader.LoadAsync(strLength);
                taskLoad.AsTask().Wait();
                uint numStrBytes = taskLoad.GetResults();
                string message = dataReader.ReadString(numStrBytes);
                _requesterReader.ReadInput(message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            finally
            {
                ReadInput();
            }
        }

        async private void WriteMessage(string message)
        {
            using (var writer = new DataWriter(_socket.OutputStream))
            {
                var len = writer.MeasureString(message);
                writer.WriteInt32((int)len);
                writer.WriteString(message);
                var ret = await writer.StoreAsync();
                writer.DetachStream();
            }
        }

        async public void Finish()
        {
            await _socket.CancelIOAsync();
        }
    }
}
