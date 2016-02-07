using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThingSockets.External
{
    public interface IRequesterActions
    {
        string Message { get; }

        void ReadMessageResult(string result);
    }
}
