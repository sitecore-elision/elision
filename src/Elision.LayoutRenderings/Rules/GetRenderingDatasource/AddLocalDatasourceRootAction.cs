using Sitecore.Data;

namespace Elision.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class AddLocalDatasourceRootAction<T> : AddDatasourceRootAction<T> where T : GetRenderingDatasourceRuleContext
    {
        public override void Apply(T ruleContext)
        {
            var contextItem = ruleContext.Args.RenderingItem.Database.GetItem(ruleContext.Args.ContextItemPath);
            if (contextItem == null)
                return;

            var folderPath = ruleContext.Settings.LocalDatasourceFolderPath;
            var dsTemplateId = ruleContext.Args.RenderingItem.Fields["Datasource template"].Value;
            var dsTemplate = ID.IsID(dsTemplateId)
                ? ruleContext.Args.ContentDatabase.GetItem(ID.Parse(dsTemplateId))
                : ruleContext.Args.RenderingItem.Database.GetItem(dsTemplateId);

            if (ruleContext.Settings.LocalDatasourceFolderNesting && dsTemplate != null)
                folderPath += "/" + dsTemplate.Name;

            var folderTemplate = ruleContext.Args.ContentDatabase.GetTemplate(ID.Parse(ruleContext.Settings.LocalDatasourceTemplateId));

            BuildNewDatasourceRoot(folderPath, folderTemplate, contextItem, "Page {0}", dsTemplate);

            base.Apply(ruleContext);
        }
    }
}
