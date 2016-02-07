using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingSockets.External
{
    public interface IListenerAddress
    {
        string Ip { get; }

        string Port { get; }
    }
}
