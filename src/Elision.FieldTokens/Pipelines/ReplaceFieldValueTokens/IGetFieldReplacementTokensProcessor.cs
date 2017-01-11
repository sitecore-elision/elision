using System.Text.RegularExpressions;

namespace Elision.Foundation.FieldTokens.Pipelines.ReplaceFieldValueTokens
{
    public interface IReplaceFieldValueTokensProcessor
    {
		void Process(ReplaceFieldValueTokensArgs args);
    }

    public abstract class ReplaceFieldValueTokensProcessorBase : IReplaceFieldValueTokensProcessor
    {
        public abstract void Process(ReplaceFieldValueTokensArgs args);

        protected string ReplaceTokenWithOptionalFormat(string value, string token, object replacementValue)
        {
            var regexSafeToken = token.Replace("[", @"\[").Replace("]", @"\]");
            return Regex.Replace(value, @"{" + regexSafeToken + "(?<fmt>:.*?)?}", m => string.Format("{0" + m.Groups["fmt"].Value + "}", replacementValue), RegexOptions.IgnoreCase);
        }
    }
}