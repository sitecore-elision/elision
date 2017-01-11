using System.Linq;
using Elision.Foundation.Kernel;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Elision.Foundation.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class AddWebsiteDatasourceRootAction<T> : AddDatasourceRootAction<T> where T : GetRenderingDatasourceRuleContext
    {
        public override void Apply(T ruleContext)
        {
            var contextItem = ruleContext.Args.RenderingItem.Database.GetItem(ruleContext.Args.ContextItemPath);
            if (contextItem == null)
                return;

            var websiteFolder = contextItem.Axes.SelectSingleItem("ancestor-or-self::*[@@TemplateID='" + Templates.Website.TemplateId + "']");

			if (websiteFolder == null)
				websiteFolder = GetWebsiteFolderForTemplateItem(contextItem, ruleContext.Args.RenderingItem.Database);

			if (websiteFolder == null)
                return;

            var folderPath = ruleContext.Settings.WebsiteDatasourceFolderPath;
            var dsTemplateId = ruleContext.Args.RenderingItem.Fields["Datasource template"].Value;
            var dsTemplate = ID.IsID(dsTemplateId)
                                  ? ruleContext.Args.ContentDatabase.GetItem(ID.Parse(dsTemplateId))
                                  : ruleContext.Args.RenderingItem.Database.GetItem(dsTemplateId);

            if (ruleContext.Settings.WebsiteDatasourceFolderNesting && dsTemplate != null)
                folderPath += "/" + dsTemplate.Name;

            var folderTemplate = ruleContext.Args.ContentDatabase.GetTemplate(ID.Parse(ruleContext.Settings.WebsiteDatasourceTemplateId));

            BuildNewDatasourceRoot(folderPath, folderTemplate, websiteFolder, "Website Shared {0}", dsTemplate);

            base.Apply(ruleContext);
        }

	    private Item GetWebsiteFolderForTemplateItem(Item contextItem, Database database)
	    {
		    var namespaceFolder = GetBranchFolderAncestor(contextItem) ?? GetTemplateFolderAncestor(contextItem);
			return namespaceFolder != null ? database.GetItem("/sitecore/content/" + namespaceFolder.Name) : null;
	    }

		private Item GetTemplateFolderAncestor(Item contextItem)
		{
			var templateFolders = contextItem.Axes.SelectItems("ancestor-or-self::*[@@templatename='Template Folder']");
			return templateFolders.Any() ? templateFolders.Last() : null;
		}

	    private Item GetBranchFolderAncestor(Item contextItem)
		{
			return contextItem.Axes.SelectSingleItem("ancestor-or-self::*[@@templatename='Branch Folder']");
		}
    }
}
