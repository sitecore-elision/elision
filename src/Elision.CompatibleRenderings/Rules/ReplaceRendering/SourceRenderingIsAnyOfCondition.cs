using Sitecore.Data;

namespace Elision.CompatibleRenderings.Rules.ReplaceRendering
{
    public class SourceRenderingIsAnyOfCondition<T> : RenderingIsAnyOfCondition<T> where T : ReplaceRenderingRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            if (ruleContext.SourceRendering == null || !ID.IsID(ruleContext.SourceRendering.ItemID)) 
                return false;

            return CompareId(ID.Parse(ruleContext.SourceRendering.ItemID));
        }
    }
}
