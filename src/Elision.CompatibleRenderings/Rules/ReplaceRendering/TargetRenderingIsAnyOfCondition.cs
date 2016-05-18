namespace Elision.CompatibleRenderings.Rules.ReplaceRendering
{
    public class TargetRenderingIsAnyOfCondition<T> : RenderingIsAnyOfCondition<T> where T : ReplaceRenderingRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            if (ruleContext.TargetRenderingItem == null)
                return false;

            return CompareId(ruleContext.TargetRenderingItem.ID);
        }
    }
}
