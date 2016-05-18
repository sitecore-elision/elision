using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Layouts;

namespace Elision.CompatibleRenderings.Rules.ReplaceRendering
{
    public class ReplaceRenderingRuleContext : RenderingRuleContext
    {
        public readonly RenderingDefinition SourceRendering;
        public readonly Item TargetRenderingItem;
        public readonly DeviceDefinition Device;

        public ReplaceRenderingRuleContext(RenderingDefinition sourceRendering, Item targetRenderingItem, DeviceDefinition device) 
            : base(new ID(sourceRendering.ItemID))
        {
            SourceRendering = sourceRendering;
            TargetRenderingItem = targetRenderingItem;
            Device = device;
        }
    }
}
