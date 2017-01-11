using Sitecore.Rules.Conditions;

namespace Elision.Foundation.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class DatasourceRootsNotSetCondition<T> : WhenCondition<T> where T : GetRenderingDatasourceRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            return ruleContext.Args.DatasourceRoots.Count == 0;
        }
    }
}