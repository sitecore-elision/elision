using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Elision.UpdateReferences
{
    internal static class Helper
    {
        public static string Replace(this string source, string oldValue, string newValue, bool ignoreCase)
        {
            if (String.IsNullOrEmpty(source))
                return source;

            if (!ignoreCase)
                return source.Replace(oldValue, newValue);

            oldValue = oldValue.Replace("[", @"\[").Replace("]", @"\]");
            return new Regex(oldValue, RegexOptions.IgnoreCase)
                .Replace(source, newValue);
        }

        public static StringBuilder Replace(this StringBuilder source, string oldValue, string newValue, bool ignoreCase)
        {
            return new StringBuilder(Replace(source.ToString(), oldValue, newValue, ignoreCase));
        }
    }
}
