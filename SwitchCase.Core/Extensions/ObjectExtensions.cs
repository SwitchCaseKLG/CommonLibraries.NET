using Newtonsoft.Json;

namespace SwitchCase.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T CloneOrDefault<T>(this T obj) where T : new()
        {
            return obj.Clone() ?? new T();
        }

        public static T? Clone<T>(this T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj), new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace });
        }

        public static T OrDefault<T>(this T? obj) where T : new()
        {
            return obj ?? new T();
        }

        public static bool IsIn<T>(this T value, params T[] values)
        {
            return values.Contains(value);
        }

        public static bool IsNull<T>(this T? value) where T : class
        {
            return value is null;
        }
        public static bool IsNotNull<T>(this T? value) where T : class
        {
            return value is not null;
        }
    }
}
