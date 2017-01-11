using System;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore.Layouts;
using Sitecore.Rules.Actions;

namespace Elision.Foundation.CompatibleRenderings.Rules.ReplaceRendering
{
    public class MoveRenderingsToNewPlaceholderAction<T> : RuleAction<T> where T : ReplaceRenderingRuleContext
    {
        public override void Apply(T ruleContext)
        {
            Guid renderingUniqueIdGuid;

            if (string.IsNullOrWhiteSpace(SourcePlaceholderKey)
                || string.IsNullOrWhiteSpace(TargetPlaceholderKey)
                || !Guid.TryParse(ruleContext.SourceRendering.UniqueId, out renderingUniqueIdGuid))
                return;

            foreach (var rendering in ruleContext.Device.Renderings.Cast<RenderingDefinition>().Where(x => !string.IsNullOrWhiteSpace(x.Placeholder)))
            {
                rendering.Placeholder = Regex.Replace(rendering.Placeholder,
                    $@"\b{SourcePlaceholderKey}_({renderingUniqueIdGuid})",
                    $"{TargetPlaceholderKey}_$1");
            }
        }

        public string SourcePlaceholderKey { get; set; }
        public string TargetPlaceholderKey { get; set; }
    }
}
