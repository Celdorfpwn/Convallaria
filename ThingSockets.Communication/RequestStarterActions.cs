using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.External;
using ThingSockets.Factory;

namespace ThingSockets.Communication
{
    class RequestStarterActions : IRequesterActions
    {

        private string _ip { get; set; }

        private IRequesterActions _actions { get; set; }

        public RequestStarterActions(string ip, IRequesterActions actions)
        {
            _ip = ip;
            _actions = actions;
        }

        public string Message
        {
            get
            {
                return "port";
            }

        }

        public void ReadMessageResult(string result)
        {
            var requester = SocketsFactory.Instance.CreateStreamRequester();
            var connectionInfo = new ConnectionInfo { Ip = _ip, Port = result };
            Debug.WriteLine("Starting requst for " + _ip + ":" + result);
            requester.Request(_actions, connectionInfo);
        }
    }
}
