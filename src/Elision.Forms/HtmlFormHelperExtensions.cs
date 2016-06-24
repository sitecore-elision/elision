using System.Web;
using System.Web.Mvc;

namespace Elision.Forms
{
    public static class HtmlFormHelperExtensions
    {
        public static HtmlString FormIdentifier(this HtmlHelper helper, string controller, string action)
        {
            return new HtmlString($"<input type=\"hidden\" name=\"formController\" value=\"{controller}\" /><input type=\"hidden\" name=\"formAction\" value=\"{action}\" />");
        }
    }
}
