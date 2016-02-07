using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.Components;
using ThingSockets.External;
using ThingSockets.Factory;

namespace ThingSockets.Communication
{
    public sealed class Communication
    {

        const int DEFAULT_PORT = 1111;

        private IListener _factoryListener { get; set; }

        private ListenerStarterActions _listenerStarter { get; set; }


        public Communication(IListenerActions deviceActions)
        {
            _listenerStarter = new ListenerStarterActions(deviceActions);
            _factoryListener = SocketsFactory.Instance.CreateListener();
            _factoryListener.Start(DEFAULT_PORT, _listenerStarter);
        }

        public Communication()
        {

        }

        public void StartStreamRequest(string ip,IRequesterActions actions)
        {
            var requestStarter = new RequestStarterActions(ip, actions);
            var requester = SocketsFactory.Instance.CreateRequester();
            var connectionInfo = new ConnectionInfo { Ip = ip, Port = DEFAULT_PORT.ToString() };
            requester.Request(requestStarter, connectionInfo);
        }



    }
}
