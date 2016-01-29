using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingSockets
{
    public interface IRequesterInfo
    {
        string Ip { get; }

        string Port { get; }
    }
}
