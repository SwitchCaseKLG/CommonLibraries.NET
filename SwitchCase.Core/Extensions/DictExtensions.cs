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

        public static TV GetOrCreateValue<TK, TV>(this Dictionary<TK, TV> dict, TK key) where TV : new() where TK : notnull
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            return dict[key] = new TV();
        }

        public static string GetValueOrKey(this Dictionary<string, string> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            else
            {
                return key;
            }
        }
    }
}
