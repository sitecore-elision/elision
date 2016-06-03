using Elision.Rules;
using Sitecore.Data;

namespace Elision.LayoutRenderings.Rules.RenderingInformation
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
