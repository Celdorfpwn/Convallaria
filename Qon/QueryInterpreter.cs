using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Qon
{
    internal class QueryInterpreter
    {
        public string GetJson(string query,object dataProvider)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            foreach (var property in query.Split(';'))
            {
                var value = GetValue(property,dataProvider);

                if (!String.IsNullOrEmpty(value))
                {
                    properties.Add(property, value);
                }
            }
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            if (properties.Count != 0)
            {

                return JsonConvert.SerializeObject(properties);
            }
            else
            {
                return String.Empty;
            }
        }

        private string GetValue(string propertyName,object dataProvider)
        {
            var property = dataProvider.GetType().GetProperty(propertyName);
            if (property != null)
            {
                return property.GetValue(dataProvider, null).ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
