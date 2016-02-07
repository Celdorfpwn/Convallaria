using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingSockets.External
{
    public interface IListenerActions
    {
        string Response(string message);
    }
}
