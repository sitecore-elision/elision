using System.Web;

namespace Elision.SiteResources
{
    public class SiteResource
    {
        public string Key { get; set; }
        public HtmlString Value { get; set; }

        public SiteResource(string key, HtmlString value)
        {
            Key = key;
            Value = value;
        }

        public SiteResource(string key, string value) : this(key, new HtmlString(value))
        {
        }
    }
}