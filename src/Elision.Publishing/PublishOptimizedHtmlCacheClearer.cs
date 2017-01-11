using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Eventing.Remote;
using Sitecore.Events;
using Sitecore.Publishing;
using Sitecore.Sites;

namespace Elision.Foundation.Publishing
{
    public class PublishOptimizedHtmlCacheClearer : HtmlCacheClearer
    {
        public new void ClearCache(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            var publishArgs = GetPublishEventArgs(args);
            if (publishArgs == null)
            {
                Log.Warn(this + " unable to parse event args, falling back to default cache clearer functionality.", this);
                base.ClearCache(sender, args);
                return;
            }

            var affectedSites = GetSitesAffectedByPublish(publishArgs).ToList();
            if (!affectedSites.Any())
            {
                Log.Warn(this + " no managed sites affected by publish. Skipping cache clearing step.", this);
                return;
            }

            Log.Info(
                this + " clearing HTML caches for sites affected by publish (" +
                string.Join(", ", affectedSites.Select(x => x.Name)) + ").", this);

            foreach (var site in affectedSites)
            {
                CacheManager.GetHtmlCache(site)?.Clear();
            }
        }

        protected virtual PublishEndRemoteEventArgs GetPublishEventArgs(EventArgs args)
        {
            var publishArgs = args as PublishEndRemoteEventArgs;
            if (publishArgs != null)
                return publishArgs;

            var scArgs = args as SitecoreEventArgs;

            var publisher = scArgs?.Parameters[0] as Publisher;
            return publisher != null
                ? new PublishEndRemoteEventArgs(new PublishEndRemoteEvent(publisher))
                : null;
        }

        protected virtual IEnumerable<SiteContext> GetSitesAffectedByPublish(PublishEndRemoteEventArgs args)
        {
            var targetDb = string.IsNullOrWhiteSpace(args.TargetDatabaseName) ? null : Factory.GetDatabase(args.TargetDatabaseName);
            var siteNames = Sites.Count > 0 ? Sites.Cast<string>().ToArray() : Factory.GetSiteNames();
            var sites = siteNames.Select(Factory.GetSite).Where(x => x != null);

            var rootItemId = new ID(args.RootItemId);
            Item rootItem = null;
            if (targetDb != null && !ID.IsNullOrEmpty(rootItemId))
                rootItem = targetDb.GetItem(rootItemId);
            if (rootItem == null)
                Log.Info(this + " : There is no root item for publish action. This will result in more aggressive cache clearing.", this);
            else if (!rootItem.Paths.Path.ToLowerInvariant().StartsWith("/sitecore/content/"))
            {
                Log.Info(this + " : Publish root is not under the content tree. Cache clearing will not consider publish root when filtering sites.", this);
                rootItem = null;
            }

            foreach (var site in sites)
            {
                if (!site.CacheHtml)
                {
                    Log.Debug(this + " : output caching not enabled for " + site.Name, this);
                    continue;
                }
                if (targetDb != null && site.Database != null && targetDb.Name != site.Database.Name)
                {
                    Log.Debug(this + " : " + targetDb + " not relevent to " + site.Name, this);
                    continue;
                }
                if (rootItem != null && !rootItem.Paths.FullPath.StartsWith(site.RootPath, StringComparison.CurrentCultureIgnoreCase))
                {
                    Log.Debug(this + " : " + site.Name  + " does not contain publish root item.", this);
                    continue;
                }
                yield return site;
            }
        }
    }
}
