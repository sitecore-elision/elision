using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetPlaceholderRenderings;

namespace Elision.Foundation.DynamicPlaceholders
{
    public class GetDynamicKeyAllowedRenderings : GetAllowedRenderings
    {
        public const string DynamicKeyPattern = @"(?<key>.+)_(?<uid>[\d\w]{8}\-(?:[\d\w]{4}\-){3}[\d\w]{12})";

        public new void Process(GetPlaceholderRenderingsArgs args)
        {
            var placeholderKey = args.PlaceholderKey;
            var regex = new Regex(DynamicKeyPattern);

            var match = regex.Match(placeholderKey);
            if (!match.Success)
                return;

            placeholderKey = match.Groups["key"].Value;
                
            Item placeholderItem = null;
            if (ID.IsNullOrEmpty(args.DeviceId))
            {
                placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase,
                    args.LayoutDefinition);
            }
            else
            {
                using (new DeviceSwitcher(args.DeviceId, args.ContentDatabase))
                    placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase,
                        args.LayoutDefinition);
            }

            List<Item> renderings = null;
            if (placeholderItem != null)
            {
                bool allowedControlsSpecified;
                args.HasPlaceholderSettings = true;
                renderings = this.GetRenderings(placeholderItem, out allowedControlsSpecified);
                if (allowedControlsSpecified)
                {
                    args.CustomData["allowedControlsSpecified"] = true;
                    args.Options.ShowTree = false;
                }
            }

            if (renderings != null)
            {
                if (args.PlaceholderRenderings == null)
                    args.PlaceholderRenderings = new List<Item>();

                args.PlaceholderRenderings.AddRange(renderings);
            }
        }
    }
}