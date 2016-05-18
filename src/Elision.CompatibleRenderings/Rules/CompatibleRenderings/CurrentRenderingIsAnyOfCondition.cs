namespace Elision.CompatibleRenderings.Rules.CompatibleRenderings
{
    public class CurrentRenderingIsAnyOfCondition<T> : RenderingIsAnyOfCondition<T> where T : GetCompatibleRenderingsRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            return ruleContext.CurrentRendering != null
                   && CompareId(ruleContext.CurrentRendering.RenderingID);
        }
    }
}
