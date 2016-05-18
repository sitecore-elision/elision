using System.Text.RegularExpressions;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;

namespace Elision.Mvc.Pipelines.GetAreaAndNamespaces
{
    public class GetAreaFromViewRenderingPath : IAreaResolveStrategy
    {
        public string Resolve(RenderRenderingArgs args)
        {
            if (args.Rendering.RenderingType != "ViewRenderer" || string.IsNullOrWhiteSpace(args.Rendering["ViewPath"]))
                return null;

            var path = args.Rendering["ViewPath"];
            var areaName = GetAreaName(path);

            return string.IsNullOrWhiteSpace(areaName) 
                ? null 
                : areaName;
        }

        protected virtual string GetAreaName(string renderingPath)
        {
            var m = Regex.Match(renderingPath,
                                @"\/areas\/(?<areaname>[^\s\/]*)\/views\/(\/[\w\s\-\+]+\/)*(.*\.(cshtml|ascx)$)",
                                RegexOptions.IgnoreCase);

            return m.Success ? m.Groups["areaname"].Value : null;
        }
    }
}