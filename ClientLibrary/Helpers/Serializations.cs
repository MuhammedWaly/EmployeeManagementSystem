using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientLibrary.Helpers
{
    public static class Serializations
    {
        public static string SerializeObj<T>(T Modelobject) => JsonSerializer.Serialize(Modelobject);
        public static T DesrializeJsonString<T>(string jsonstring) => JsonSerializer.Deserialize<T>(jsonstring);
        public static IList<T> DesrializeJsonStringList<T>(string jsonstring) => JsonSerializer.Deserialize<IList<T>>(jsonstring);
    }
}
