using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Pipelines;
using Sitecore.Pipelines.ExecutePageEditorAction;

namespace Elision.Foundation.CompatibleRenderings.Pipelines.ExecutePageEditorAction
{
    public class InsertRendering : Sitecore.Pipelines.ExecutePageEditorAction.InsertRendering
    {
        public new void Process(PipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            var insertRenderingArgs = args as ExecuteInsertRenderingArgs;
            if (insertRenderingArgs == null)
                return;

            var renderingItem = insertRenderingArgs.RenderingItem;
            Assert.IsNotNull(renderingItem, "renderingItem");

            var placeholderKey = insertRenderingArgs.PlaceholderKey;
            Assert.IsNotNullOrEmpty(placeholderKey, "placeholderKey");

            var renderingDefinition = new RenderingDefinition()
            {
                ItemID = renderingItem.ID.ToString(),
                Placeholder = placeholderKey,
                //the next line is the only code change from the base class
                Parameters = renderingItem["Parameters"]
            };
            if (insertRenderingArgs.Datasource != null)
                renderingDefinition.Datasource = insertRenderingArgs.Datasource.ID.ToString();

            Assert.IsNotNull(insertRenderingArgs.Device, "device");

            InsertRenderingAt(insertRenderingArgs.Device, renderingDefinition, insertRenderingArgs.Position, insertRenderingArgs.AllowedRenderingsIds);
            insertRenderingArgs.Result = new RenderingReference(renderingDefinition, insertRenderingArgs.Language, insertRenderingArgs.ContentDatabase);
        }
    }
}