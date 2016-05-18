using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls;

namespace Elision.TabbedRenderingSelector
{
    public class TabbedSelectRenderingForm : Sitecore.Shell.Applications.Dialogs.SelectRendering.SelectRenderingForm
    {
        protected Tabstrip Tabs;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;

            SelectRenderingOptions renderingOptions = SelectItemOptions.Parse<SelectRenderingOptions>();
            if (renderingOptions.ShowTree)
            {
                var gridPanel = Renderings.Parent as GridPanel;
                gridPanel?.SetExtensibleProperty(Tabs, "class", "scDisplayNone");
            }
            else
            {
                var groupedRenderings = renderingOptions.Items.GroupBy(i => i.Parent.Name).ToArray();
                if (groupedRenderings.Length > 1)
                {
                    foreach (var renderingGroup in groupedRenderings)
                    {
                        var newTab = new Tab
                        {
                            Header = renderingGroup.Key,
                            CssClass = "scScrollbox scFixSize scFixSize4",
                            Height = new Unit(100, UnitType.Percentage),
                            Width = new Unit(100, UnitType.Percentage)
                        };
                        var newScrollbox = new Scrollbox
                        {
                            Class = "scScrollbox scFixSize scFixSize4",
                            Background = "white",
                            Padding = "0px",
                            Width = new Unit(100, UnitType.Percentage),
                            Height = new Unit(100, UnitType.Percentage),
                            InnerHtml = RenderPreviews(renderingGroup)
                        };
                        newTab.Controls.Add(newScrollbox);
                        Tabs.Controls.Add(newTab);
                    }
                }
            }
        }

        private string RenderPreviews(IEnumerable<Item> renderingGroup)
        {
            var privateBaseMethod = typeof (Sitecore.Shell.Applications.Dialogs.SelectRendering.SelectRenderingForm)
                .GetMethod("RenderPreviews", BindingFlags.NonPublic | BindingFlags.Instance);
            return (string) privateBaseMethod.Invoke(this, new object[] {renderingGroup});
        }
    }
}
