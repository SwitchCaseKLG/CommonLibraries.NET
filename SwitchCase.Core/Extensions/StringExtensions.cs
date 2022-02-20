namespace SwitchCase.Core.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return Enum.TryParse<T>(value, true, out T result) ? result : defaultValue;
        }

        public static string GetNumbers(this string input)
        {
            return input.Filter(char.IsDigit);
        }

        public static string Filter(this string input, Func<char, bool> filter)
        {
            return new string(input.Where(filter).ToArray());
        }

        public static T Parse<T>(this string input, T defaultValue)
        {
            T value;

            if (string.IsNullOrWhiteSpace(input))
            {
                value = defaultValue;
            }
            else
            {
                try
                {
                    value = (T)Convert.ChangeType(input, typeof(T));
                }
                catch (Exception)
                {
                    value = defaultValue;
                }
            }
            return value;
        }
    }
}
