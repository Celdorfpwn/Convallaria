using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ThingSockets;

namespace Qon
{
    public sealed class QueryReader : IListenerIOActions
    {
        private object _dataProvider { get; set; }

        private List<string> _properties { get; set; }

        public QueryReader(object dataProvider)
        {
            _dataProvider = dataProvider;
            _properties = new List<string>();
        }

        public void ReadInput(string message)
        {
            _properties.AddRange(message.Split(';'));
        }

        public string GetOutput()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            foreach(var property in _properties)
            {
                properties.Add(property, GetValue(property));
            }

            return JsonConvert.SerializeObject(properties);
        }

        private string GetValue(string propertyName)
        {
            return _dataProvider.GetType().GetProperty(propertyName).GetValue(_dataProvider, null).ToString();
        }
    }
}
