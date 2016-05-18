using Elision.LookupSourceItems.Pipelines.GetLookupSourceItems;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetLookupSourceItems;

namespace Elision.Themes.GetLookupSourceItems
{
    public class ReplpaceThemeItemToken : ReplaceLookupSourceQueryTokens
    {
        private readonly IThemeRetriever _themeRetriever;

        public ReplpaceThemeItemToken(IThemeRetriever themeRetriever)
        {
            _themeRetriever = themeRetriever;
        }

        public override void Process(GetLookupSourceItemsArgs args)
        {
            args.Source = ReplaceThemeItemToken(args.Source, args.Item);
        }

        protected virtual string ReplaceThemeItemToken(string query, Item contextItem)
        {
            var token = "{Theme}";
            if (!query.Contains(token)) return query;

            var themeItem = _themeRetriever.GetThemeFromContextItem(contextItem);
            if (themeItem == null) return query;

            return query.Replace(token, themeItem.Paths.FullPath);
        }
    }
}
