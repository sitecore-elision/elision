using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls;

namespace Elision.Foundation.TabbedRenderingSelector
{
    public class TabbedSelectRenderingForm : Sitecore.Shell.Applications.Dialogs.SelectRendering.SelectRenderingForm
    {
        protected Tabstrip Tabs;

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull((object) e, "e");
            IsOpenPropertiesChecked = Registry.GetBool("/Current_User/SelectRendering/IsOpenPropertiesChecked");
            SelectRenderingOptions renderingOptions = SelectItemOptions.Parse<SelectRenderingOptions>();
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;

            var groupedRenderings = renderingOptions.Items?.GroupBy(i => i.Parent.Name).ToArray() ?? new IGrouping<string, Item>[0];
            if (renderingOptions.ShowTree || groupedRenderings.Length == 0)
            {
                var gridPanel = Renderings.Parent as GridPanel;
                gridPanel?.SetExtensibleProperty(Tabs, "class", "scDisplayNone");
            }
            else
            {
                foreach (var renderingGroup in groupedRenderings)
                {
                    var newTab = new Tab
                    {
                        Header = renderingGroup.Key,
                        CssClass = "scTabPage"
                    };
                    newTab.Controls.Add(new LiteralControl(RenderPreviews(renderingGroup)));
                    Tabs.Controls.Add(newTab);
                }

                Renderings.Visible = false;
                var gridPanel = Renderings.Parent as GridPanel;
                gridPanel?.SetExtensibleProperty(Renderings, "class", "scDisplayNone");
            }
        }

        private string RenderPreviews(IEnumerable<Item> renderingGroup)
        {
            var privateBaseMethod = typeof (Sitecore.Shell.Applications.Dialogs.SelectRendering.SelectRenderingForm)
                .GetMethod("RenderPreviews", BindingFlags.NonPublic | BindingFlags.Instance, null, new [] { renderingGroup.GetType() }, null);
            return (string) privateBaseMethod.Invoke(this, new object[] {renderingGroup});
        }
    }
}
