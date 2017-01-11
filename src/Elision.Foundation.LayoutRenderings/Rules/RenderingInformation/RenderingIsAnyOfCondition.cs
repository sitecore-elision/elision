using System.Linq;
using Sitecore.Data;
using Sitecore.Rules.Conditions;

namespace Elision.Foundation.LayoutRenderings.Rules.RenderingInformation
{
    public class RenderingIsAnyOfCondition<T> : WhenCondition<T> where T : RenderingRuleContext
    {
        public string CompareRenderingItemIds { get; set; }
        protected override bool Execute(T ruleContext)
        {
            return CompareId(ruleContext.RenderingId);
        }

        protected virtual bool CompareId(ID renderingId)
        {
            if (string.IsNullOrWhiteSpace(CompareRenderingItemIds))
                return false;

            return ID.ParseArray(CompareRenderingItemIds)
                .Any(compareToId => ID.Parse(compareToId) == renderingId);
        }
    }
}