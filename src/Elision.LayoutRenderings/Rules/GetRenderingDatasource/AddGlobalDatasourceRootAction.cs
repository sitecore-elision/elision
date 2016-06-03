using Sitecore;
using Sitecore.Data;

namespace Elision.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class AddGlobalDatasourceRootAction<T> : AddDatasourceRootAction<T> where T : GetRenderingDatasourceRuleContext
    {
        public override void Apply(T ruleContext)
        {
            var contextItem = ruleContext.Args.RenderingItem.Database.GetItem(ruleContext.Args.ContextItemPath);
            if (contextItem == null)
                return;

            contextItem = contextItem.Database.GetItem(ItemIDs.ContentRoot);
            if (contextItem == null)
                return;

            var folderPath = ruleContext.Settings.GlobalDatasourceFolderPath;
            var dsTemplateId = ruleContext.Args.RenderingItem.Fields["Datasource template"].Value;
            var dsTemplate = ID.IsID(dsTemplateId)
                                  ? ruleContext.Args.ContentDatabase.GetItem(ID.Parse(dsTemplateId))
                                  : ruleContext.Args.RenderingItem.Database.GetItem(dsTemplateId);

            if (ruleContext.Settings.GlobalDatasourceFolderNesting && dsTemplate != null)
                folderPath += "/" + dsTemplate.Name;

            var folderTemplate = ruleContext.Args.ContentDatabase.GetTemplate(ID.Parse(ruleContext.Settings.GlobalDatasourceTemplateId));

            BuildNewDatasourceRoot(folderPath, folderTemplate, contextItem, "Global {0}", dsTemplate);

            base.Apply(ruleContext);
        }
    }
}
