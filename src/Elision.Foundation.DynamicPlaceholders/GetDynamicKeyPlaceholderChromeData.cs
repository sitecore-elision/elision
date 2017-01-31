using System;
using System.Text.RegularExpressions;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;

namespace Elision.Foundation.DynamicPlaceholders
{
    public class GetDynamicKeyPlaceholderChromeData : GetPlaceholderChromeData
    {
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");

            if (!"placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase)) 
                return;

            var placeholderKey = args.CustomData["placeHolderKey"] as string;
            var regex = new Regex(GetDynamicKeyAllowedRenderings.DynamicKeyPattern);
            var match = regex.Match(placeholderKey ?? "");

            if (match.Success)
            {
                var newPlaceholderKey = match.Groups["key"].Value;
                args.CustomData["placeHolderKey"] = newPlaceholderKey;

                base.Process(args);
                args.CustomData["placeHolderKey"] = placeholderKey;
            }
            else
            {
                base.Process(args);
            }
        }
    }
}