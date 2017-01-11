using System.Collections.Generic;

namespace Elision.Foundation.Mvc.Pipelines.GetControllerRenderingValueParameters
{
    public class AddSitecoreContextParameters : IGetControllerRenderingValueParametersProcessor
    {
        public void Process(GetControllerRenderingValueParametersArgs args)
        {
            var parameters = new Dictionary<string, object>
                {
                    {"database", Sitecore.Context.Database},
                    {"db", Sitecore.Context.Database}
                };

            foreach (var parameter in parameters)
            {
                if (!args.Parameters.ContainsKey(parameter.Key))
                    args.Parameters.Add(parameter.Key, parameter.Value);
            }
        }
    }
}