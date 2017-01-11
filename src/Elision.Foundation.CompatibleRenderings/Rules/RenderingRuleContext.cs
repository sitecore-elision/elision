using Elision.Foundation.Rules;
using Sitecore.Data;

namespace Elision.Foundation.CompatibleRenderings.Rules
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
