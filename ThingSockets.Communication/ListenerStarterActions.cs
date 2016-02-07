using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.External;
using System.Net.NetworkInformation;
using ThingSockets.Factory;
using ThingSockets.Components;

namespace ThingSockets.Communication
{
    sealed class ListenerStarterActions : IListenerActions
    {
        private int _currentPort { get; set; }
        private IListenerActions _actions { get; set; }

        private List<IListener> _listeners { get; set; }

        public ListenerStarterActions(IListenerActions actions)
        {
            _actions = actions;
            _listeners = new List<IListener>();
            _currentPort = 2000;
        }

        public string Response(string message)
        {
            return GetOpenPort();
        }


        private string GetOpenPort()
        {

            IListener listener = _listeners.FirstOrDefault(existingListener => !existingListener.HasConnection);

            if(listener!=null)
            {
                return listener.Port;
            }

            listener = SocketsFactory.Instance.CreateStreamListener();
            while(true)
            {
                try
                {
                    listener.Start(_currentPort++, _actions);
                    _listeners.Add(listener);
                    return listener.Port;
                }
                catch
                {

                }
            }
        }
    }
}
