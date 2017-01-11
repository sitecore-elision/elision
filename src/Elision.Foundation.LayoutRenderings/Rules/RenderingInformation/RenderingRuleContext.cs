using Elision.Foundation.Rules;
using Sitecore.Data;

namespace Elision.Foundation.LayoutRenderings.Rules.RenderingInformation
{
    public class RenderingRuleContext : EnhancedRuleContext
    {
        public readonly ID RenderingId;

        public RenderingRuleContext(ID renderingId)
        {
            RenderingId = renderingId;
        }
    }
}
