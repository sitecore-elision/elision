using Sitecore.Rules.Conditions;

namespace Elision.Foundation.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class RenderingCondition<T> : WhenCondition<T> where T : GetRenderingDatasourceRuleContext
    {
        public string Rendering { get; set; }

        protected override bool Execute(T ruleContext)
        {
            if (Rendering == null || ruleContext.Args.RenderingItem == null)
                return false;
            return ruleContext.Args.RenderingItem.ID.ToString() == Rendering;
        }
    }
}