using System.Linq;

namespace Elision.Seo.Pipelines.GenerateSitemapXml
{
    public class StoreSitemapsCache : IGenerateSitemapProcessor
    {
        public void Process(GenerateSitemapArgs args)
        {
            //if (args.SitemapFiles == null || !args.SitemapFiles.Any())
            //    return;

            //for (var i = 0; i < args.SitemapFiles.Count; i++)
            //{
            //    if (i == 0)
            //        SitemapXmlCache.Current.Set(args.CacheKeyBase, new SitemapXmlFile(args.SitemapFiles[i]));
            //    else
            //        SitemapXmlCache.Current.Set(args.CacheKeyBase + "_" + i, new SitemapXmlFile(args.SitemapFiles[i]));
            //}
        }
    }
}
