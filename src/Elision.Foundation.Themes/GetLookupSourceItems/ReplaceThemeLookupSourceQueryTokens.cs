using Elision.Foundation.LookupSourceItems.Pipelines.GetLookupSourceItems;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetLookupSourceItems;

namespace Elision.Foundation.Themes.GetLookupSourceItems
{
    public class ReplaceThemeItemToken : ReplaceLookupSourceQueryTokens
    {
        private readonly IThemeRetriever _themeRetriever;

        public ReplaceThemeItemToken(IThemeRetriever themeRetriever)
        {
            _themeRetriever = themeRetriever;
        }

        public override void Process(GetLookupSourceItemsArgs args)
        {
            args.Source = ReplaceThemeToken(args.Source, args.Item);
        }

        protected virtual string ReplaceThemeToken(string query, Item contextItem)
        {
            var token = "{Theme}";
            if (!query.Contains(token)) return query;

            var themeItem = _themeRetriever.GetThemeFromContextItem(contextItem);
            if (themeItem == null) return query;

            return query.Replace(token, themeItem.Paths.FullPath);
        }
    }
}
