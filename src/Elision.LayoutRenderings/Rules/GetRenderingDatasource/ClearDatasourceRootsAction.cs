using Sitecore.Rules.Actions;

namespace Elision.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class ClearDatasourceRootsAction<T> : RuleAction<T> where T : GetRenderingDatasourceRuleContext
    {
        public override void Apply(T ruleContext)
        {
            ruleContext.Args.DatasourceRoots.Clear();
        }
    }
}
