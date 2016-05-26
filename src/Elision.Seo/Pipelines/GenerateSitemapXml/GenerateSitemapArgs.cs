using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Elision.Seo.Pipelines.GenerateSitemapXml
{
    public class GenerateSitemapArgs : PipelineArgs
    {
        public Item RootItem { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public List<string> SitemapFiles { get; set; }

        public string CacheKeyBase { get; set; }

        public string Content { get; set; }

        public string RequestUrl { get; set; }
    }
}
