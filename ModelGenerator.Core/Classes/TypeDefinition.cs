using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelGenerator.Core.Classes
{
    public class TypeDefinition
    {
        public string[] Keys { get; set; }
        public string Value { get; set; }
        public bool TryGetValue(string key, out string value)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            if (Keys.Contains(key))
            {
                value = Value;
                return true;
            }
            return false;
        }
        public static TypeDefinition[] LoadFromResource(string resource)
        {
            if (resource.EndsWith("json"))
            {
                var contents = File.ReadAllText(resource);
                return JsonConvert.DeserializeObject<TypeDefinition[]>(contents);
            }
            else
            {
                return JsonConvert.DeserializeObject<TypeDefinition[]>(resource);
            }
        }
    }
}
