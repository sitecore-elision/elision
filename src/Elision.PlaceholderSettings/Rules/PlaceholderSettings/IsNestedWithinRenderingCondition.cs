using System.Linq;
using Sitecore.Data;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Presentation;
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

            var id = ID.Parse(RenderingItemId);
            var isNested = ContextService.Get().GetInstances<RenderingContext>()
                                         .Any(x => x.Rendering.RenderingItem.ID == id);
            return isNested;
        }
    }
}