using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Rules;

namespace Elision.Rules
{
    public interface IRulesRunner
    {
        void RunGlobalRules<T>(Item rulesFolder, T context) where T : EnhancedRuleContext;
        void RunGlobalRules<T>(ID rulesFolderId, Database db, T context) where T : EnhancedRuleContext;
        void RunGlobalRules<T>(string rulesFolderName, Database db, T context) where T : EnhancedRuleContext;
    }

    public class RulesRunner : IRulesRunner
    {
        public void RunGlobalRules<T>(Item rulesFolder, T context) where T : EnhancedRuleContext
        {
            if (rulesFolder == null)
            {
                Log.SingleError("Rules folder not found:\r\n" + Environment.StackTrace, this);
                return;
            }

            foreach (var ruleItem in rulesFolder.Axes.GetDescendants())
            {
                var ruleXml = ruleItem["Rule"];
                if (string.IsNullOrWhiteSpace(ruleXml) || ruleItem["Disabled"] == "1")
                    continue;

                var rules = new EnhancedRuleList<T>(RuleFactory.ParseRules<T>(ruleItem.Database, ruleXml).Rules);

                rules.Run(context);

                if (context.StopProcessingThisRuleset)
                {
                    context.StopProcessingThisRuleset = false;
                    typeof(RuleContext).GetProperty("IsAborted").SetMethod.Invoke(context, new object[] {false});
                    continue;
                }
                if (context.StopProcessingAfterThisRuleset)
                {
                    context.StopProcessingAfterThisRuleset = false;
                    context.GetType().GetProperty("IsAborted").SetMethod.Invoke(context, new object[] {false});
                    break;
                }
                if (context.IsAborted)
                    break;
            }
        }

        public void RunGlobalRules<T>(ID rulesFolderId, Database db, T context) where T : EnhancedRuleContext
        {
            Assert.ArgumentNotNull(db, "db");
            Assert.IsFalse(ID.IsNullOrEmpty(rulesFolderId), "rulesFolderId not specified");

            var rulesFolder = db.GetItem(rulesFolderId);
            if (rulesFolder == null)
            {
                Log.SingleError($"Rules folder for id '{rulesFolderId}' not found in '{db.Name}' database", this);
                return;
            }

            RunGlobalRules(rulesFolder, context);
        }

        public void RunGlobalRules<T>(string rulesFolderName, Database db, T context) where T : EnhancedRuleContext
        {
            Assert.ArgumentNotNull(db, "db");
            Assert.ArgumentNotNullOrEmpty(rulesFolderName, "rulesFolderName");

            var rulesFolder = db.GetItem(rulesFolderName)
                              ?? db.GetItem($"/sitecore/system/rules/{rulesFolderName}/Rules");
            if (rulesFolder == null)
            {
                Log.SingleError($"Rules folder '{rulesFolderName}' not found in '{db.Name}' database", this);
                return;
            }

            RunGlobalRules(rulesFolder, context);
        }
    }
}
