using System.Web;
using Elision.Diagnostics;
using Sitecore.Links;

namespace Elision.Seo.Pipelines.GetCanonicalUrl
{
    public class GetCanonicalUrlForAlias : IGetCanonicalUrlProcessor
    {
        public void Process(GetCanonicalUrlArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.CanonicalUrl))
                return;

            using (var trace = new TraceOperation("GetCanonicalUrlForAlias"))
            {
                if (!Sitecore.Configuration.Settings.AliasesActive)
                {
                    trace.Debug("Skipping because AliasesActive = false");
                    return;
                }
                if (!Sitecore.Context.Database.Aliases.Exists(args.RawUrl))
                {
                    trace.Debug($"Skipping because alias not found in database '{Sitecore.Context.Database.Name}' for raw url '{args.RawUrl}'");
                    return;
                }

                var targetId = Sitecore.Context.Database.Aliases.GetTargetID(args.RawUrl);
                var targetItem = Sitecore.Context.Database.GetItem(targetId);
                if (targetItem == null)
                {
                    trace.Warning($"Unable to find the item '{targetId}' defined in the alias.");
                    return;
                }

                args.CanonicalUrl = HttpContext.Current != null
                    ? $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Host}{LinkManager.GetItemUrl(targetItem)}"
                    : LinkManager.GetItemUrl(targetItem);

                trace.Info($"Setting canonical url to '{args.CanonicalUrl}'");
            }
        }
    }
}