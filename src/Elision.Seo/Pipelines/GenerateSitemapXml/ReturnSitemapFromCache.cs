namespace Elision.Seo.Pipelines.GenerateSitemapXml
{
    public class ReturnSitemapFromCache : IGenerateSitemapProcessor
    {
        public void Process(GenerateSitemapArgs args)
        {
            //if (string.IsNullOrWhiteSpace(args.CacheKeyBase))
            //    return;

            //var sitemap = SitemapXmlCache.Current.Get(args.CacheKeyBase);
            //if (sitemap != null)
            //    args.Content = sitemap.Content;
        }
    }
}
