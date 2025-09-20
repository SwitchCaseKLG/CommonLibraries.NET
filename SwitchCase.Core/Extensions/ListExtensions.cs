namespace SwitchCase.Core.Extensions
{
    public static class ListExtensions
    {
        public static void AddIfNotExist<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
            }
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }

        public static bool IsNotEmpty<T>(this List<T> list)
        {
            return list.Count > 0;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return !list.Any();
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> list)
        {
            return list.Any();
        }

        public static string StringJoin(this IEnumerable<string> list, string seperator = "")
        {
            return string.Join(seperator, list);
        }

        public static string StringJoin<T>(this IEnumerable<T> list, Func<T, string> toStringFunc, string seperator = "")
        {
            return string.Join(seperator, list.Select(toStringFunc));
        }
    }
}
