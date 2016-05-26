namespace Elision.Seo.Pipelines.GenerateSitemapXml
{
    public class GetSitemapRoot : IGenerateSitemapProcessor
    {
        public void Process(GenerateSitemapArgs args)
        {
            if (args.RootItem == null)
                args.RootItem = Sitecore.Context.Site.GetStartItem();
        }
    }
}
