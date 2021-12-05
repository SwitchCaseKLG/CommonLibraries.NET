namespace SwitchCase.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Dedupe<T, Tkey>(this IEnumerable<T> items, Func<T, Tkey> keySelector)
        {
            return items.GroupBy(keySelector).Select(y => y.First());
        }

        public static Dictionary<Tkey, T> ToDedupedDictionary<Tkey, T>(this IEnumerable<T> items, Func<T, Tkey> keySelector) where Tkey : notnull
        {
            //return items.Dedupe(keySelector).ToDictionary(keySelector); //simpler, but keySelector function get calculated twice
            return items.Select(i => new KeyValuePair<Tkey, T>(keySelector(i), i)).Dedupe(i => i.Key).ToDictionary(i => i.Key, i => i.Value);
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new();
            foreach (T element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
