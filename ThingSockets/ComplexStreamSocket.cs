using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ThingSockets
{
    class ComplexStreamSocket
    {
        public DataReader DataReader { get; private set; }

        public StreamSocket Socket { get; private set; }

        public DataWriter DataWriter { get; private set; }

        public ComplexStreamSocket(StreamSocket socket)
        {
            Socket = socket;
            DataReader = new DataReader(socket.InputStream);
            DataWriter = new DataWriter(socket.OutputStream);
        }

    }
}
