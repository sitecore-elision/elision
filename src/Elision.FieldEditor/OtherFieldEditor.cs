using System.Linq;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Elision.FieldEditor
{
    public class OtherFieldEditor : Sitecore.Shell.Applications.WebEdit.Commands.FieldEditor
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            var item = ResolveDatasource(context)
                ?? context.Items.FirstOrDefault();

            if (item == null)
                return;

            Context.ClientPage.Start(this, "StartFieldEditor", new ClientPipelineArgs(context.Parameters)
                {
                    Parameters = {{"uri", item.Uri.ToString()}}
                });
        }

        protected Item ResolveDatasource(CommandContext context)
        {
            if (context.Items.Length < 1) return null;

            var datasourceString = context.Parameters["datasource"];
            var contextItem = context.Items.First();
            return contextItem.Database.ResolveDatasource(datasourceString, contextItem);
        }
    }
}
