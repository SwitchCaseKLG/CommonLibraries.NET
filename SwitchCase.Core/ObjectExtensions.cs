using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitchCase.Core
{
    public static class ObjectExtensions
    {
        public static T CloneOrDefault<T>(this T obj) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj), new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace }) ?? new T();
        }

        public static T? Clone<T>(this T obj) where T: class
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj), new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace });
        }
    }
}
