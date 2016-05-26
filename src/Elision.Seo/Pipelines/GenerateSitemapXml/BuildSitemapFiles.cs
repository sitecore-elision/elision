using System;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Elision.Seo.Pipelines.GenerateSitemapXml
{
    public class BuildSitemapFiles : IGenerateSitemapProcessor
    {
        public virtual void Process(GenerateSitemapArgs args)
        {
            var sitemap = new StringBuilder();
            var items = args.Items.ToList();

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];

                var itemXml = BuildUrlNode(item);

                if ((i > 0 && (i%50000) == 0) //sitemap file cannot contain more than 50,000 urls
                    || (sitemap.Length + itemXml.Length) > 2097152 //sitemap file cannot be larger than 10MB, each UTF-8 character could be as many as 6 bytes -- this comparison is the lazy way to accomplish this
                    )
                {
                    sitemap.Append(BuildSitemapFileFooter());
                    args.SitemapFiles.Add(sitemap.ToString());

                    sitemap = new StringBuilder();
                    sitemap.Append(BuildSitemapFileHeader());
                }
                sitemap.Append(itemXml);
            }

            if (args.SitemapFiles.Count > 1)
                args.SitemapFiles.Insert(0, BuildSitemapIndexFile(args));
        }

        protected virtual string BuildSitemapIndexFile(GenerateSitemapArgs args)
        {
            var contents = new StringBuilder();
            contents.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                            "<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\r\n");

            for (var i = 0; i < args.SitemapFiles.Count; i++)
            {
                var filename = args.CacheKeyBase + "_" + i + ".xml";
                contents.AppendFormat(
                    "\t<sitemap><loc>{0}</loc><lastmod>{1:yyyy-MM-ddTHH:mm:ss+00:00}</lastmod></sitemap>\r\n",
                    filename,
                    DateTime.UtcNow
                    );
            }

            contents.Append("</sitemapindex>");

            return contents.ToString();
        }

        protected virtual string BuildSitemapFileHeader()
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine +
                   "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">";
        }

        protected virtual string BuildSitemapFileFooter()
        {
            return "</urlset>";
        }

        protected virtual string BuildUrlNode(Item item)
        {
            var urlOptions = UrlOptions.DefaultOptions;
            urlOptions.AlwaysIncludeServerUrl = true;

            return string.Format("<url><loc>{0}</loc><lastmod>{1:yyyy-MM-dd}</lastmod></url>",
                                 /* <changefreq>monthly</changefreq><priority>0.5</priority> */
                                 LinkManager.GetItemUrl(item, urlOptions),
                                 item.Statistics.Updated
                );
        }
    }
}
