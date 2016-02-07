
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ThingSockets.External;

namespace Qon
{
    sealed class QueryReader : IListenerActions
    {
        private object _dataProvider { get; set; }

        private List<string> _properties { get; set; }

        private QueryInterpreter _interpreter { get; set; }

        public QueryReader(object dataProvider)
        {
            _dataProvider = dataProvider;
            _interpreter = new QueryInterpreter();
        }

        public string Response(string message)
        {
            return _interpreter.GetJson(message, _dataProvider);
        }

    }
}
