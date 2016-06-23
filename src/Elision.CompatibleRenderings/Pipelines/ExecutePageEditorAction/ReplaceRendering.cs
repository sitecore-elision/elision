using Elision.CompatibleRenderings.Rules.ReplaceRendering;
using Elision.Rules;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;

namespace Elision.CompatibleRenderings.Pipelines.ExecutePageEditorAction
{
    public class ReplaceRendering : Sitecore.Pipelines.ExecutePageEditorAction.ReplaceRendering
    {
        private readonly IRulesRunner _rulesRunner;

        public ReplaceRendering(IRulesRunner rulesRunner)
        {
            _rulesRunner = rulesRunner;
        }

        protected override RenderingDefinition DoReplaceRendering(RenderingDefinition sourceRendering, Item targetRenderingItem, DeviceDefinition device)
        {
            Assert.ArgumentNotNull(sourceRendering, "sourceRendering");
            Assert.ArgumentNotNull(targetRenderingItem, "targetRenderingItem");
            Assert.ArgumentNotNull(device, "device");
            var renderingDefinition = new RenderingDefinition
                {
                    Cachable = sourceRendering.Cachable,
                    Conditions = sourceRendering.Conditions,
                    Datasource = sourceRendering.Datasource,
                    ItemID = targetRenderingItem.ID.ToString(),
                    MultiVariateTest = sourceRendering.MultiVariateTest,
                    Parameters = sourceRendering.Parameters,
                    Placeholder = sourceRendering.Placeholder,
                    Rules = sourceRendering.Rules,
                    VaryByData = sourceRendering.VaryByData,
                    ClearOnIndexUpdate = sourceRendering.ClearOnIndexUpdate,
                    VaryByDevice = sourceRendering.VaryByDevice,
                    VaryByLogin = sourceRendering.VaryByLogin,
                    VaryByParameters = sourceRendering.VaryByParameters,
                    VaryByQueryString = sourceRendering.VaryByQueryString,
                    VaryByUser = sourceRendering.VaryByUser,
                    DynamicProperties = sourceRendering.DynamicProperties,
                    // need to make sure the unique id of the original rendering gets copied over to the new one
                    // so that we don't lose any items
                    UniqueId = sourceRendering.UniqueId
                };
            if (device.Renderings != null)
            {
                var index = device.Renderings.IndexOf(sourceRendering);
                device.Renderings.RemoveAt(index);
                device.Renderings.Insert(index, renderingDefinition);
            }

            RunReplaceRenderingRules(sourceRendering, targetRenderingItem, device);
            return renderingDefinition;
        }

        private void RunReplaceRenderingRules(RenderingDefinition sourceRendering, Item targetRenderingItem, DeviceDefinition device)
        {
            var context = new ReplaceRenderingRuleContext(sourceRendering, targetRenderingItem, device);
            _rulesRunner.RunGlobalRules("Replace Rendering", targetRenderingItem.Database, context);
        }
    }
}