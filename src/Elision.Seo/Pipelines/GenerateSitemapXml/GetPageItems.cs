using System.Linq;

namespace Elision.Seo.Pipelines.GenerateSitemapXml
{
    public class GetPageItems : IGenerateSitemapProcessor
    {
        public void Process(GenerateSitemapArgs args)
        {
            args.Items = new[] {args.RootItem}.Union(args.RootItem.Axes.GetDescendants());
        }
    }
}
