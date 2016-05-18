using System.Web.Mvc;
using System.Web.UI;
using Sitecore.Data;

namespace Elision.Mvc
{
    public static class EditFrameExtensions
    {
        public static EditFrame BeginEditFrame(this HtmlHelper htmlHelper)
        {
            return BeginEditFrame(htmlHelper, "");
        }

        public static EditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString());
        }

        public static EditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource)
        {
            return BeginEditFrame(htmlHelper, dataSource, "/sitecore/content/Applications/WebEdit/Edit Frame Buttons/Default");
        }

        public static EditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, ID buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons.ToString());
        }

        public static EditFrame BeginEditFrame(this HtmlHelper htmlHelper, ID dataSource, string buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource.ToString(), buttons);
        }

        public static EditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, ID buttons)
        {
            return BeginEditFrame(htmlHelper, dataSource, buttons.ToString());
        }

        public static EditFrame BeginEditFrame(this HtmlHelper htmlHelper, string dataSource, string buttons)
        {
            var editFrame = new EditFrame(buttons, htmlHelper.ViewContext.Writer, dataSource);
            editFrame.RenderFirstPart();
            return editFrame;
        }

        public static EditFrame BeginEditFrame(this HtmlHelper htmlHelper, Sitecore.Web.UI.WebControls.EditFrame editFrame)
        {
            var output = new HtmlTextWriter(htmlHelper.ViewContext.Writer);
            editFrame.RenderFirstPart(output);
            return new EditFrame(editFrame);
        }
    }
}