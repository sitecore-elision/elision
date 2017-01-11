using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;

namespace Elision.Foundation.WebEditState
{
    public class CheckFieldWebEdit
    {
        public void Process(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            if (!args.Parameters.ContainsKey("disable-web-editing") && WebEditStateSwitcher.CurrentValue == WebEditState.Disabled)
            {
                args.Parameters.Add("disable-web-editing", "true");
            }
        }
    }
}
