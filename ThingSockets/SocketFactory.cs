using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingSockets
{
    public class SocketFactory
    {

        static SocketFactory()
        {
            _instance = new SocketFactory();
        }

        private SocketFactory()
        {

        }


        public IListener CreateListener(IListenerIOActions socketIOActions)
        {
            return new Listener(socketIOActions);
        }

        public IRequester CreateRequester(IRequesterInfo requesterInfo,IRequesterReader requesterReader)
        {
            return new Requester(requesterInfo, requesterReader);
        }


        private static SocketFactory _instance { get; set; }

        public static SocketFactory Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
