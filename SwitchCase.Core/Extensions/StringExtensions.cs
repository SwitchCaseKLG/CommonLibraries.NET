using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SwitchCase.Core.Extensions
{
    public static class StringExtensions
    {
        private readonly static string[] TRUE_VALUES =
        [
            "true", "t", "wahr", "w", "ja", "j", "yes", "y"
        ];

        private readonly static string[] FALSE_VALUES =
        [
            "false", "f", "falsch", "nein", "n", "no"
        ];

        public static string TrimEnd(this string value, string trimString)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(trimString))
            {
                return value;
            }

            return value.EndsWith(trimString, StringComparison.Ordinal) ? value[..^trimString.Length] : value;
        }

        public static string SplitToLines(this string stringToSplit, int maximumLineLength)
        {
            return Regex.Replace(stringToSplit, @"(.{1," + maximumLineLength + @"})(?:\s|$)", "$1\n").TrimEnd('\n');
        }

        public static bool IsEmpty(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotEmpty(this string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string OrEmpty(this string? value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return value;
        }

        public static string IfEmpty(this string? value, string defaultValue = "-")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            return value;
        }

        public static int ToInt(this string? value)
        {
            return int.TryParse(value, out int result) ? result : 0;
        }

        public static bool? ToBool(this string? value)
        {
            if (value == null) return null;
            if (TRUE_VALUES.Contains(value.ToLower())) return true;
            if (FALSE_VALUES.Contains(value.ToLower())) return false;
            return null;
        }

        public static bool ToBool(this string? value, bool defaultValue = false)
        {
            if (value == null) return defaultValue;
            if (TRUE_VALUES.Contains(value.ToLower())) return true;
            if (FALSE_VALUES.Contains(value.ToLower())) return false;
            return defaultValue;
        }

        public static string ToSingleLineAndQuoted(this string value)
        {
            value ??= string.Empty;
            return value.Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").Replace("\"", "'").Trim().Enquote();
        }

        public static string Enquote(this string value)
        {
            return $"\"{value}\"";
        }

        public static string NormalizeString(this string value)
        {
            if (value == null) return string.Empty;

            // 1. Unicode-Normalisierung (NFD), trennt Akzentzeichen von Buchstaben
            string decomposed = value.Normalize(NormalizationForm.FormD);

            // 2. Entferne diakritische Zeichen
            StringBuilder sb = new();
            foreach (char c in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            // 3. In Großbuchstaben umwandeln
            return sb.ToString().ToUpperInvariant();
        }

        public static string Shrink(this string value, int limit, string delimiter = "…")
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            int length = value.Length;
            int length2 = delimiter.Length;

            if (limit < length2 + 1)
            {
                return "";
            }

            if (limit >= length)
            {
                return value;
            }

            return value[..(limit - length2)] + delimiter;
        }

        public static T ToEnum<T>(this string? value, T defaultValue) where T : struct
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
            return new string([.. input.Where(filter)]);
        }

        public static T Parse<T>(this string? input, T defaultValue)
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

        public static string Limit(this string value, int length)
        {
            if (value.Length < length)
            {
                return value;
            }
            if (length > 3)
            {
                return value[..(length - 3)] + "...";
            }
            else
            {
                return value[..length];
            }
        }
    }
}
