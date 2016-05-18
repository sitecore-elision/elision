using System.Web;

namespace Elision.Areas.Elision.Models
{
    public class SiteResourceViewModel
    {
        public HtmlString SiteResources { get; set; }

        public HtmlString PageResources { get; set; }

        public HtmlString ThemeResources { get; set; }
    }
}