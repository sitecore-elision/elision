using Elision.LayoutRenderings.Rules.RenderingInformation;
using Sitecore.Pipelines.GetRenderingDatasource;

namespace Elision.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class GetRenderingDatasourceRuleContext : RenderingRuleContext
    {
        public GetRenderingDatasourceArgs Args { get; set; }

        public RenderingDatasourceRulesSettings Settings { get; set; }

        public GetRenderingDatasourceRuleContext(GetRenderingDatasourceArgs args) 
            : base(args.RenderingItem.ID)
        {
            Settings = new RenderingDatasourceRulesSettings();

            Args = args;
            Item = args.RenderingItem;
        }
    }
}