namespace SwitchCase.Core.Extensions
{
    public static class DictExtensions
    {
        public static T GetOrCreateValue<T>(this Dictionary<string, T> dict, string key) where T : new()
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                T value = new();
                dict[key] = value;
                return value;
            }
        }
    }
}
