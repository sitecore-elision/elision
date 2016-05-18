using System.Collections.Generic;
using System.Linq;
using System.Web;
using Elision.PlaceholderSettings.Rules.PlaceholderSettings;
using Elision.Rules;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetPlaceholderRenderings;

namespace Elision.PlaceholderSettings.Pipelines.GetPlaceholderRenderings
{
    public class RunPlaceholderSettingsRules
    {
        private readonly IRulesRunner _rulesRunner;

        public RunPlaceholderSettingsRules(IRulesRunner rulesRunner)
        {
            _rulesRunner = rulesRunner;
        }

        public void Process(GetPlaceholderRenderingsArgs args)
        {
            if (args.PlaceholderRenderings == null)
                args.PlaceholderRenderings = new List<Item>();

            var hasInitialRenderings = args.PlaceholderRenderings.Any();

            _rulesRunner.RunGlobalRules("Placeholder Settings",
                                        args.ContentDatabase,
                                        new PlaceholderSettingsRuleContext(args, GetContextItem(args)));

            if (hasInitialRenderings && args.PlaceholderRenderings.Count == 0)
            {
                args.HasPlaceholderSettings = false;
                args.PlaceholderRenderings = null;
            }
            else if (!hasInitialRenderings && args.PlaceholderRenderings.Count > 0)
            {
                args.HasPlaceholderSettings = true;
                args.Options.ShowTree = false;
            }
        }

        private static Item GetContextItem(GetPlaceholderRenderingsArgs args)
        {
            var item = Context.Item;
            if (item == null)
            {
                var editingItemId = HttpContext.Current.Request.QueryString["id"];
                if (ID.IsID(editingItemId))
                    item = args.ContentDatabase.GetItem(ID.Parse(editingItemId));
            }
            return item;
        }
    }
}
