using System;
using System.IO;
using System.Web.UI;

namespace Elision.Foundation.Mvc
{
    public class EditFrame : IDisposable
    {
        public const string DefaultEditButtons = "/sitecore/content/Applications/WebEdit/Edit Frame Buttons/Default";
        private readonly Sitecore.Web.UI.WebControls.EditFrame _frame;
        private readonly HtmlTextWriter _writer;

        public EditFrame(string buttons, TextWriter writer, string dataSource = DefaultEditButtons)
        {
            this._frame = new Sitecore.Web.UI.WebControls.EditFrame
                {
                    DataSource = dataSource, 
                    Buttons = buttons
                };
            this._writer = new HtmlTextWriter(writer);
        }

        public EditFrame(Sitecore.Web.UI.WebControls.EditFrame frame)
        {
            this._frame = frame;
        }

        public void RenderFirstPart()
        {
            this._frame.RenderFirstPart(this._writer);
        }

        public void Dispose()
        {
            this._frame.RenderLastPart(this._writer);
            this._writer.Dispose();
        }
    }
}
