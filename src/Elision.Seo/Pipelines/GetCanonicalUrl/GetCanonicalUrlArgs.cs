using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Elision.Seo.Pipelines.GetCanonicalUrl
{
    public class GetCanonicalUrlArgs : PipelineArgs
    {
        public GetCanonicalUrlArgs(Item pageItem, string rawUrl)
        {
            PageItem = pageItem;
            RawUrl = rawUrl;
        }

        public string CanonicalUrl { get; set; }
        public string RawUrl { get; set; }

        public Item PageItem { get; set; }
    }
}