namespace Elision.Foundation.Kernel
{
    public static class StringExtensions
    {
        public static string Or(this string value, string fallbackValue)
        {
            return string.IsNullOrWhiteSpace(value) ? fallbackValue : value;
        }
    }
}