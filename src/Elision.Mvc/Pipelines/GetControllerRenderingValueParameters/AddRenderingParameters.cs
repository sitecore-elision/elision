using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;

namespace Elision.Mvc.Pipelines.GetControllerRenderingValueParameters
{
    public class AddRenderingParameters : IGetControllerRenderingValueParametersProcessor
    {
        public void Process(GetControllerRenderingValueParametersArgs args)
        {
            var parameters = new Dictionary<string, object>();
            var renderingContext = args.RenderingContext;
            foreach (var parameter in renderingContext.Rendering.Parameters)
            {
                parameters.Add(parameter.Key, parameter.Value);
                if (parameter.Value == "1")
                    parameters.Add(parameter.Key + "Bool", true);
                else if (parameter.Value == "0" || string.IsNullOrWhiteSpace(parameter.Value))
                    parameters.Add(parameter.Key + "Bool", false);

                if (renderingContext.ContextItem == null) continue;

                var items = renderingContext.ContextItem.Database.ResolveDatasourceItems(parameter.Value, renderingContext.PageContext.Item)
                                    .ToArray();
                if (!parameters.ContainsKey(parameter.Key + "Items"))
                    parameters.Add(parameter.Key + "Items", items);
                if (items.Any() && !parameters.ContainsKey(parameter.Key + "Item"))
                    parameters.Add(parameter.Key + "Item", items.First());
            }

            parameters.Add("rendering", renderingContext.Rendering);
            parameters.Add("renderingUniqueId", new ID(renderingContext.Rendering.UniqueId));
            parameters.Add("pageContext", renderingContext.PageContext);
            parameters.Add("renderingDataSource", renderingContext.Rendering.DataSource);
            parameters.Add("renderingContextItem", renderingContext.ContextItem);
            parameters.Add("pageContextItem", renderingContext.PageContext.Item);

            foreach (var parameter in parameters)
            {
                if (!args.Parameters.ContainsKey(parameter.Key))
                    args.Parameters.Add(parameter.Key, parameter.Value);
            }
        }
    }
}
