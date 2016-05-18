using System.Text.RegularExpressions;
using Sitecore.Pipelines.GetPlaceholderRenderings;

namespace Elision.DynamicPlaceholders
{
    public class GetAllowedRenderingsSetPlaceholderKey : GetAllowedRenderings
    {
        public new void Process(GetPlaceholderRenderingsArgs args)
        {
            var regex = new Regex(GetDynamicKeyAllowedRenderings.DynamicKeyPattern);

            var match = regex.Match(args.PlaceholderKey);
            if (!match.Success) 
                return;

            var placeholderKey = match.Groups["key"].Value;

            if (!args.CustomData.ContainsKey("DynamicPlaceholderKey"))
                args.CustomData.Add("DynamicPlaceholderKey", args.PlaceholderKey);

            var placeholderKeyField = args.GetType().GetField("placeholderKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            placeholderKeyField?.SetValue(args, placeholderKey);
        }
    }
}