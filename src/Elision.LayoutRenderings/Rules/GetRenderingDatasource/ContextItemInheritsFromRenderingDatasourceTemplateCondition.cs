using Sitecore.Rules.Conditions;

namespace Elision.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class ContextItemInheritsFromRenderingDatasourceTemplateCondition<T> : WhenCondition<T> where T : GetRenderingDatasourceRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            var contextItem = ruleContext.Args.RenderingItem.Database.GetItem(ruleContext.Args.ContextItemPath);
            if (contextItem == null)
                return false;

            var dsTemplate = ruleContext.Args.ContentDatabase.ResolveDatasource(ruleContext.Args.RenderingItem["Datasource template"], contextItem);
            if (dsTemplate == null)
                return false;

            return contextItem.InheritsFrom(dsTemplate.ID);
        }
    }
}