using Elision.Diagnostics;
using Elision.LayoutRenderings.Rules.GetRenderingDatasource;
using Elision.Rules;
using Sitecore.Pipelines.GetRenderingDatasource;

namespace Elision.LayoutRenderings.Pipelines.GetRenderingDatasource
{
    public class RunGetDatasourceLocationRules
    {        
        private readonly IRulesRunner _rulesRunner;
        public RunGetDatasourceLocationRules(IRulesRunner rulesRunner)
        {
            _rulesRunner = rulesRunner;
        }

        public void Process(GetRenderingDatasourceArgs args)
        {
            SetDatasourceRoots(args);
        }

        private void SetDatasourceRoots(GetRenderingDatasourceArgs args)
        {
            using (new TraceOperation("Run GetDatasourceLocation rules"))
            {
                _rulesRunner.RunGlobalRules(RulesFolders.Get_Rendering_Datasource,
                                            args.RenderingItem.Database,
                                            new GetRenderingDatasourceRuleContext(args));
            }
        }
    }
}
