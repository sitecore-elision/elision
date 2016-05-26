namespace Elision.Seo.Pipelines.GenerateSitemapXml
{
    public class AbortPipelineIfNoCacheKey : IGenerateSitemapProcessor
    {
        public void Process(GenerateSitemapArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.CacheKeyBase))
                args.AbortPipeline();
        }
    }
}
