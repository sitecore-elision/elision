using Elision.Rules;
using Sitecore.Data;

namespace Elision.CompatibleRenderings.Rules
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
