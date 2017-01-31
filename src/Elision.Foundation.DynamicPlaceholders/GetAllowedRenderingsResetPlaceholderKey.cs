using Sitecore.Pipelines.GetPlaceholderRenderings;

namespace Elision.Foundation.DynamicPlaceholders
{
    public class GetAllowedRenderingsResetPlaceholderKey : GetAllowedRenderings
    {
        public new void Process(GetPlaceholderRenderingsArgs args)
        {
            if (!args.CustomData.ContainsKey("DynamicPlaceholderKey"))
                return;

            var placeholderKey = args.CustomData["DynamicPlaceholderKey"] as string;

            var placeholderKeyField = args.GetType().GetField("placeholderKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            placeholderKeyField?.SetValue(args, placeholderKey);
        }
    }
}