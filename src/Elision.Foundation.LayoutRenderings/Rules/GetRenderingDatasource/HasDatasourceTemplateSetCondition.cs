using Sitecore.Rules.Conditions;

namespace Elision.Foundation.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class HasDatasourceTemplateSetCondition<T> : WhenCondition<T> where T : GetRenderingDatasourceRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            return !string.IsNullOrWhiteSpace(ruleContext.Args.RenderingItem.Fields["Datasource template"].Value);
        }
    }
}