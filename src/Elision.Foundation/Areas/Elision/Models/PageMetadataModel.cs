using System;
using Sitecore.Data.Items;

namespace Elision.Foundation.Areas.Elision.Models
{
    public class PageMetadataModel
    {
        public Item InnerItem { get; set; }

        public string BrowserTitle { get; set; }
        public string CanonicalUrl { get; set; }

        public string OgSiteName { get; set; }
        public string OgType { get; set; }
        public string OgTitle { get; set; }
        public string OgDescription { get; set; }
        public OgImageModel OgImage { get; set; }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public string Language { get; set; }
        public DateTime LastModified { get; set; }
        public string Url { get; set; }

        public string RobotsMeta { get; set; }
    }
}