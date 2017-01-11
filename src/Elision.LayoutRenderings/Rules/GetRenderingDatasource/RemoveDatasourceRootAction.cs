using System.Linq;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Diagnostics;
using Sitecore.Rules.Actions;
using Sitecore.Sites;
using Sitecore.Web;

namespace Elision.Foundation.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class RemoveDatasourceRootAction<T> : RuleAction<T> where T : GetRenderingDatasourceRuleContext
    {
        public string DatasourceRootId { get; set; }
        public override void Apply(T ruleContext)
        {
            if (!ID.IsID(DatasourceRootId))
                return;

            var matchingRoot = ruleContext.Args.DatasourceRoots.FirstOrDefault(x => x.ID.ToString() == DatasourceRootId);
            if (matchingRoot != null)
            {
                Log.Debug($"Removing {matchingRoot.DisplayName} from datasource roots for rendering {ruleContext.Args.RenderingItem.DisplayName} ({ruleContext.Args.RenderingItem.ID}).", this);
                ruleContext.Args.DatasourceRoots.Remove(matchingRoot);
                if (!matchingRoot.HasChildren)
                    using (new SiteContextSwitcher(SiteContextFactory.GetSiteContext("system") ?? new SiteContext(new SiteInfo(new StringDictionary()))))
                    using (new EventDisabler())
                        matchingRoot.Delete();
            }
        }
    }
}
