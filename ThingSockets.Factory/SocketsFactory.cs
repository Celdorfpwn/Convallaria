using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.Components;

namespace ThingSockets.Factory
{
    public sealed class SocketsFactory
    {

        static SocketsFactory()
        {
            Instance = new SocketsFactory();
        }

        private SocketsFactory()
        {

        }

        public IListener CreateStreamListener()
        {
            return new StreamListener();
        }

        public IRequester CreateStreamRequester()
        {
            return new StreamRequester();
        }

        public IListener CreateListener()
        {
            return new Listener();
        }

        public IRequester CreateRequester()
        {
            return new Requester();
        }

        public static SocketsFactory Instance { get; set; }
    }
}
