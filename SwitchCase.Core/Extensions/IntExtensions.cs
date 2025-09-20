namespace SwitchCase.Core.Extensions
{
    public static class IntExtensions
    {
        public static int? NullIfZero(this int value)
        {
            return value == 0 ? null : value;
        }

        public static string IfZero(this int value, string defaultValue = "-")
        {
            return value == 0 ? defaultValue : value.ToString();
        }

        public static string IfZero(this long value, string defaultValue = "-")
        {
            return value == 0 ? defaultValue : value.ToString();
        }
    }
}
