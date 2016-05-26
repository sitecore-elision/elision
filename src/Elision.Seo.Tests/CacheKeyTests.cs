using Elision.Seo.Pipelines.GenerateSitemapXml;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Elision.Seo.Tests
{
    [TestClass]
    public class CacheKeyTests
    {
        [TestMethod]
        public void ParsesRequestIntoBaseCacheKey()
        {
            var url = "http://test.com/sitemap.xml";
            var args = new GenerateSitemapArgs()
                {
                    RequestUrl = url
                };

            new GetCacheKey().Process(args);

            args.CacheKeyBase.Should().Be("http://test.com/sitemap.xml");
        }

        [TestMethod]
        public void ParsesIndexRequestIntoBaseCacheKey()
        {
            var url = "http://test.com/sitemap_index.xml";
            var args = new GenerateSitemapArgs()
            {
                RequestUrl = url
            };

            new GetCacheKey().Process(args);

            args.CacheKeyBase.Should().Be("http://test.com/sitemap.xml");
        }
    }
}
