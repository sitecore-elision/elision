using Elision.Foundation.Kernel;
using Elision.Foundation.Kernel.Diagnostics;
using Elision.Foundation.LayoutRenderings.Rules.GetRenderingDatasource;
using Elision.Foundation.Rules;
using Sitecore.Pipelines.GetRenderingDatasource;

namespace Elision.Foundation.LayoutRenderings.Pipelines.GetRenderingDatasource
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
