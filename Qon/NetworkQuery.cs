using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.Communication;
using ThingSockets.External;

namespace Qon
{
    public sealed class NetworkQuery
    {
        private Communication _communication { get; set; }
        
        public NetworkQuery(object dataProvider)
        {
            _communication = new Communication(new QueryReader(dataProvider));
        }

        public NetworkQuery()
        {
            _communication = new Communication();
        }

        public void RunNetworkQuery(string ip,IRequesterActions actions)
        {
            _communication.StartStreamRequest(ip, actions);
        }
    }
}
