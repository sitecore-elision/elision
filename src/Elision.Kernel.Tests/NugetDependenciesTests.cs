using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using NUnit.Framework;

namespace Elision.Foundation.Kernel.Tests
{
    [TestFixture]
    public class NugetDependenciesTests
    {
        [Test]
        public void AllSitecorePackagesAreDevelopmentDependencies()
        {
            var issues = new Dictionary<string, List<string>>();

            var files = GetAllPackagesConfigFiles();
            foreach (var file in files)
            {
                issues.Add(file, new List<string>());

                var nodes = GetNodesToCheck(file);
                if (!nodes.Any())
                {
                    Console.WriteLine("INFO: No sitecore packages in {0}", file);
                    continue;
                }

                foreach (var node in nodes)
                {
                    if (node.Attributes?["developmentDependency"]?.Value == "true")
                        continue;

                    issues[file].Add(node.OuterXml);
                }
            }

            if (issues.Any(x => x.Value.Any()))
                Assert.Fail("The following sitecore dependencies are not marked as development dependencies. Since these Sitecore packages are not in a public Nuget server, this will cause problems for users.\r\n\r\n"
                            + string.Join("\r\n\r\n", issues.Where(x => x.Value.Any()).Select(x => x.Key + "\r\n\r\n" + string.Join("\r\n", x.Value))));
        }

        private static XmlNode[] GetNodesToCheck(string file)
        {
            var doc = new XmlDocument();
            doc.Load(file);

            var nodes = doc.SelectNodes("//*[starts-with(@id,'Sitecore')]");
            if (nodes == null)
                return new XmlNode[0];
            return nodes.Cast<XmlNode>()
                .Where(x => Regex.IsMatch(x?.Attributes?["id"]?.Value ?? "", @"^Sitecore\d+\w+"))
                .ToArray();
        }

        private static IEnumerable<string> GetAllPackagesConfigFiles()
        {
            var files = Directory
                .GetFiles(@"..\..\..\..\src\", @"packages.config", SearchOption.AllDirectories)
                .ToArray();

            if (files.Length == 0)
                Assert.Inconclusive("No packages files were found. Please confirm that this is correct.");
            return files;
        }
    }
}
