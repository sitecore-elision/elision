using System;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore.Data;
using Sitecore.Layouts;
using Sitecore.Rules.Actions;

namespace Elision.CompatibleRenderings.Rules.ReplaceRendering
{
    public class TransformDirectChildRenderingsAction<T> : RuleAction<T> where T : ReplaceRenderingRuleContext
    {
        public override void Apply(T ruleContext)
        {
            string[] fromRenderings;
            string toRendering;
            Guid renderingUniqueIdGuid;
            try
            {
                fromRenderings = ID.ParseArray(FromRenderingIds).Select(x => x.ToString()).ToArray();
                toRendering = ID.Parse(ToRenderingId).ToString();
                renderingUniqueIdGuid = Guid.Parse(ruleContext.SourceRendering.UniqueId);
            }
            catch
            {
                return;
            }

            var childPlaceholderKeyPattern = $@"^{ruleContext.SourceRendering.Placeholder}/[^/]+(_{renderingUniqueIdGuid.ToString().ToLowerInvariant()})?$";
            var childRenderings = ruleContext.Device.Renderings.Cast<RenderingDefinition>().Where(x => Regex.IsMatch(x.Placeholder ?? "", childPlaceholderKeyPattern)).ToArray();
            foreach (var rendering in childRenderings.Where(x => fromRenderings.Contains(x.ItemID)))
            {
                rendering.ItemID = toRendering;
            }
        }

        public string FromRenderingIds { get; set; }
        public string ToRenderingId { get; set; }
    }
}
