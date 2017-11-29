using System;
using System.Linq;
using Sitecore.Data;
using Sitecore.Layouts;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Elision.PlaceholderSettings.Rules.PlaceholderSettings
{
    public class IsNestedWithinRenderingCondition<T> : WhenCondition<T> where T : RuleContext
    {
        public string RenderingItemId { get; set; }

        protected override bool Execute(T ruleContext)
        {
            if (string.IsNullOrWhiteSpace(RenderingItemId) || !ID.IsID(RenderingItemId))
                return false;

            var enhancedContext = ruleContext as PlaceholderSettingsRuleContext;
            if (enhancedContext == null) return false;

            var layoutDefinition = LayoutDefinition.Parse(enhancedContext.Args.LayoutDefinition);

            var renderings = layoutDefinition.Devices.Cast<DeviceDefinition>()
                .Where(x => x != null)
                .SelectMany(x => x.Renderings.Cast<RenderingDefinition>())
                .ToArray();

            if (!renderings.Any())
                return false;

            var isNested = renderings.Any(x => x.ItemID.Equals(RenderingItemId, StringComparison.InvariantCultureIgnoreCase)
                                && EnsurePrefix(enhancedContext.PlaceholderKeyPath).StartsWith(EnsurePrefix(x.Placeholder))
                                && enhancedContext.FullUniquePlaceholderKey.Contains(x.UniqueId.TrimStart('{').TrimEnd('}').ToLower()));
            return isNested;
        }

        protected virtual string EnsurePrefix(string placeholder)
        {
            return placeholder.StartsWith("/") ? placeholder : "/" + placeholder;
        }
    }
}