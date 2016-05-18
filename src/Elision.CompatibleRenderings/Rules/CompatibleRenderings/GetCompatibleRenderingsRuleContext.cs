using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Layouts;

namespace Elision.CompatibleRenderings.Rules.CompatibleRenderings
{
    public class GetCompatibleRenderingsRuleContext : RenderingRuleContext
    {
        public readonly RenderingReference CurrentRendering;
        public readonly List<ID> CompatibleRenderings;

        public GetCompatibleRenderingsRuleContext(RenderingReference currentRendering, IEnumerable<ID> compatibleRenderings, Item item)
            : base(currentRendering == null ? ID.Null : currentRendering.RenderingID)
        {
            CurrentRendering = currentRendering;
            CompatibleRenderings = new List<ID>(compatibleRenderings);
            Item = item;
        }
    }
}
