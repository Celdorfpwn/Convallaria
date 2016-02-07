using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.External;
using System.Net.NetworkInformation;
using ThingSockets.Factory;

namespace ThingSockets.Communication
{
    sealed class ListenerStarterActions : IListenerActions
    {

        private IListenerActions _actions { get; set; }

        public ListenerStarterActions(IListenerActions actions)
        {
            _actions = actions;
        }

        public string Response(string message)
        {

            return GetOpenPort().ToString();
        }


        private int GetOpenPort()
        {
            var listener = SocketsFactory.Instance.CreateStreamListener();
            for (var port = 2000;port <= 3000;port++)
            {
                try
                {
                    listener.Start(port, _actions);
                    return port;
                }
                catch
                {

                }
            }

            return 0;
        }
    }
}
