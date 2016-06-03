using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Rules.Actions;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.Web;

namespace Elision.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class AddDatasourceRootAction<T> : RuleAction<T> where T : GetRenderingDatasourceRuleContext
    {
        public Item NewDatasourceRoot { get; set; }
        public string DatasourceRootId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (NewDatasourceRoot == null && !string.IsNullOrWhiteSpace(DatasourceRootId))
                NewDatasourceRoot = ruleContext.Args.RenderingItem.Database.ResolveDatasource(DatasourceRootId);

            if (NewDatasourceRoot != null)
                ruleContext.Args.DatasourceRoots.Add(NewDatasourceRoot);
        }

        protected virtual Item BuildNewDatasourceRoot(string datasourceFolderPath, TemplateItem template, Item contextItem, string folderDisplayNamePattern, TemplateItem datasourceTemplate)
        {
            NewDatasourceRoot = contextItem.Axes.SelectSingleItem(datasourceFolderPath);
            if (NewDatasourceRoot != null)
                return NewDatasourceRoot;

            NewDatasourceRoot = BuildPath(contextItem, datasourceFolderPath, template);

            if (datasourceTemplate == null)
                return NewDatasourceRoot;

            using (new SiteContextSwitcher(SiteContextFactory.GetSiteContext("system") ?? new SiteContext(new SiteInfo(new StringDictionary()))))
            using (new EditContext(NewDatasourceRoot))
            {
                var pluralizer = System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService(CultureInfo.CurrentCulture);
                NewDatasourceRoot.Appearance.DisplayName = string.Format(folderDisplayNamePattern, pluralizer.Pluralize(datasourceTemplate.DisplayName));
                NewDatasourceRoot.Appearance.Icon = datasourceTemplate.Icon;
            }
            return NewDatasourceRoot;
        }

        protected virtual Item BuildPath(Item contextItem, string localDsQuery, TemplateItem templateItem)
        {
            var folders = new Queue<string>(localDsQuery.Trim('.', '/')
                                                        .Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries));
            using (new SecurityDisabler())
            using (new SiteContextSwitcher(SiteContextFactory.GetSiteContext("system") ?? new SiteContext(new SiteInfo(new StringDictionary()))))
            {
                return BuildPath(contextItem, folders, templateItem);
            }
        }

        protected virtual Item BuildPath(Item contextItem, Queue<string> folders, TemplateItem localDsTemplate)
        {
            if (folders.Count == 0)
                return contextItem;

            var nextFolderName = folders.Dequeue();

            var child = contextItem
                            .Children
                            .FirstOrDefault(x => x.Name.Equals(nextFolderName, StringComparison.InvariantCultureIgnoreCase))
                        ?? contextItem.Add(nextFolderName, localDsTemplate);

            return BuildPath(child, folders, localDsTemplate);
        }
    }
}
