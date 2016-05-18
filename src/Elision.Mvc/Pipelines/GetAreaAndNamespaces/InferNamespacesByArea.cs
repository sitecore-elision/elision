using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Pipelines.Response.RenderRendering;
using Sitecore.Mvc.Presentation;

namespace Elision.Mvc.Pipelines.GetAreaAndNamespaces
{
    public class InferNamespacesByArea : RenderRenderingProcessor
    {
        public override void Process(RenderRenderingArgs args)
        {
            var routeData = PageContext.Current.RequestContext.RouteData;

            var areaName = routeData.DataTokens["area"] as string;
            if (string.IsNullOrWhiteSpace(areaName))
                return;

            var originalNamespaces = routeData.DataTokens["namespaces"] as IEnumerable<string>;
            var newNamespaces = GetNamespaces(areaName);

            routeData.DataTokens["namespaces"] = newNamespaces;
            
            args.Disposables.Add(new GenericDisposable(() => ResetArea(originalNamespaces)));
        }

        private void ResetArea(IEnumerable<string> originalNamespaces)
        {
            var routeData = PageContext.Current.RequestContext.RouteData;
            if (originalNamespaces != null)
                routeData.DataTokens["namespaces"] = originalNamespaces;
            else
                routeData.DataTokens.Remove("namespaces");
        }

        public virtual IEnumerable<string> GetNamespaces(string areaName)
        {
            var namespaces = GetAllNamespaces().Where(x => !string.IsNullOrWhiteSpace(x));
            namespaces = string.IsNullOrWhiteSpace(areaName)
                             ? namespaces.Where(IsControllerNamespaceWithoutArea)
                             : namespaces.Where(s => NamespaceCouldBeForArea(s, areaName));

            return namespaces;
        }

        private IEnumerable<string> _allNamespaces;
        private IEnumerable<string> GetAllNamespaces()
        {
            if (_allNamespaces == null)
            {
                var assemblies = AppDomain.CurrentDomain
                                          .GetAssemblies()
                                          .Where(x => !x.FullName.StartsWith("System.")
                                                      && !x.FullName.StartsWith("Microsoft.")
                                                      && !x.FullName.StartsWith("Sitecore."));
                var types = new List<Type>();
                foreach (var assembly in assemblies)
                {
                    try
                    {
                        types.AddRange(assembly.GetExportedTypes());
                    }
                    catch { }
                }
                _allNamespaces = types.Select(x => x.Namespace)
                                      .Where(x => !string.IsNullOrWhiteSpace(x)
                                                  && !x.StartsWith("System.")
                                                  && !x.StartsWith("Microsoft.")
                                                  && !x.StartsWith("Sitecore."))
                                      .Distinct().ToArray();
            }
            return _allNamespaces;
        }

        protected virtual bool IsControllerNamespaceWithoutArea(string namespaceName)
        {
            return Regex.IsMatch(namespaceName, @"\bControllers\b", RegexOptions.IgnoreCase) && !Regex.IsMatch(namespaceName, @"\bAreas\b", RegexOptions.IgnoreCase);
        }

        protected virtual bool NamespaceCouldBeForArea(string namespaceName, string areaName)
        {
            return Regex.IsMatch(namespaceName, @"\bAreas\." + areaName, RegexOptions.IgnoreCase);
        }
    }
}
