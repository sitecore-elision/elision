using System.Text.RegularExpressions;

namespace Elision.Seo.Pipelines.GenerateSitemapXml
{
    public class GetCacheKey : IGenerateSitemapProcessor
    {
        protected Regex SitemapUrlRegex = new Regex(
            @"^(?<base>.*[/\\]sitemap)(?<modifier>.*)(?<extension>\.xml.*)$",
            RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        public void Process(GenerateSitemapArgs args)
        {
            var match = SitemapUrlRegex.Match(args.RequestUrl);
            if (match.Success)
                args.CacheKeyBase = match.Groups["base"].Value + match.Groups["extension"].Value;
        }
    }
}
