using System;
using System.Collections.Generic;
using System.Linq;
using Elision.Foundation.Rules;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Pipelines.GetPlaceholderRenderings;

namespace Elision.Foundation.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class PlaceholderSettingsRuleContext : EnhancedRuleContext
    {
        public GetPlaceholderRenderingsArgs Args { get; set; }

        public string PlaceholderKeyPath => Args.PlaceholderKey;
        public string PlaceholderKey => Args.PlaceholderKey.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Last();

        public string FullUniquePlaceholderKey => Args.CustomData.ContainsKey("DynamicPlaceholderKey")
            ? Args.CustomData["DynamicPlaceholderKey"] as string
            : Args.PlaceholderKey;

        private DeviceItem _device;
        public DeviceItem Device
        {
            get { return _device ?? (_device = Args.ContentDatabase.GetItem(Args.DeviceId)); }
            set { _device = value; }
        }

        public PlaceholderSettingsRuleContext(GetPlaceholderRenderingsArgs args, Item contextItem)
        {
            Args = args;
            Item = contextItem;
        }

        public IEnumerable<RenderingReference> GetRenderings()
        {
            var layoutString = Args.LayoutDefinition;
            if (!string.IsNullOrWhiteSpace(layoutString))
            {
                try
                {
                    var layout = LayoutDefinition.Parse(layoutString);
                    return layout.GetDevice(Args.DeviceId.ToString())
                                 .Renderings.Cast<RenderingDefinition>()
                                 .Select(x => new RenderingReference(x, Item.Language, Args.ContentDatabase))
                                 .ToArray();
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to parse page designing layout definition \"{layoutString}\"", ex, this);
                }
            }
            return Item.Visualization.GetRenderings(Device, false);
        }
    }
}
