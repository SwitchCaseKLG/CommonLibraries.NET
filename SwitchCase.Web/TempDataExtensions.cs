using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace SwitchCase.Web
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class, new()
        {
            tempData.TryGetValue(key, out object? o);
            return o == null ? new T() : JsonSerializer.Deserialize<T>(o.ToString() ?? string.Empty) ?? new T();
        }
    }
}
