using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.External;

namespace ThingSockets.Components
{
    public interface IListener
    {
        void Start(int port,IListenerActions actions);

    }
}
