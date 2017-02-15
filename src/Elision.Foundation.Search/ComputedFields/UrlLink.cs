using Elision.Foundation.Kernel;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Sites;

namespace Elision.Foundation.Search.ComputedFields
{
    public class UrlLink : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            var item = (Item)(indexable as SitecoreIndexableItem);
            if (item == null)
                return null;
            if (item.Paths.IsMediaItem)
                return MediaManager.GetMediaUrl(item);

            var urlOptions = (UrlOptions)UrlOptions.DefaultOptions.Clone();
            urlOptions.LanguageEmbedding = LanguageEmbedding.Never;
            urlOptions.AlwaysIncludeServerUrl = true;

            var site = item.GetSite();
            if (site != null)
                urlOptions.Site = new SiteContext(site);

            return EnsurePrefix(LinkManager.GetItemUrl(item, urlOptions));
        }

        protected virtual string EnsurePrefix(string url)
        {
            if ((url ?? "").StartsWith("://"))
                return "http" + url;
            return url;
        }
    }
}
