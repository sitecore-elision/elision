namespace Elision
{
    public static class StringExtensions
    {
        public static string Or(this string value, string fallbackValue)
        {
            return string.IsNullOrWhiteSpace(value) ? fallbackValue : value;
        }
    }
}