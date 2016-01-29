using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingSockets
{
    public interface IListener
    {
        void Start(int port);
    }
}
