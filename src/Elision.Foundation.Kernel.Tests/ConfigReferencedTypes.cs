using NUnit.Framework;
using Sitecore.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace Elision.Foundation.Kernel.Tests
{
    [TestFixture]
    public class ConfigReferencedTypes
    {
        [Test]
        public void AllReferencedTypesExist()
        {
            //get reference to all config files in the main web project
            //find all nodes that reference a type
            //try to retrieve the type definition to make sure it exists

            var issues = new Dictionary<string, List<string>>();

            var files = GetAllConfigFiles();
            foreach (var file in files)
            {
                issues.Add(file, new List<string>());

                var nodesWithTypes = GetNodesToCheckForTypeRefs(file);
                if (nodesWithTypes == null || nodesWithTypes.Count == 0)
                {
                    Console.WriteLine("WARNING: No type references found in {0}", file);
                    continue;
                }

                foreach (var node in nodesWithTypes.Cast<XmlNode>())
                {
                    var typeName = GetTypeRefFromNode(node);
                    if (string.IsNullOrWhiteSpace(typeName))
                        continue;

                    try
                    {
                        var typeInfo = TypeHelper.GetType(typeName);
                        //var service = ServiceLocator.ServiceProvider.GetService(typeof(IMyService))
                        if (typeInfo == null)
                            issues[file].Add(node.OuterXml);
                        else if (System.Diagnostics.Debugger.IsAttached)
                            Console.WriteLine("INFO: Type resolved \"{0}\"", typeInfo.AssemblyQualifiedName);
                    }
                    catch (Exception ex)
                    {
                        issues[file].Add(node.OuterXml + "\r\nERROR: " + ex.Message);
                    }
                }
            }

            if (issues.Any(x => x.Value.Any()))
                Assert.Fail("Failed to find the type definitions for these types. Make sure the test project references (CopyLocal=True) all necessary Sitecore Dlls that contain all base types used.\r\n\r\n"
                            + string.Join("\r\n\r\n", issues.Where(x => x.Value.Any()).Select(x => x.Key + "\r\n\r\n" + string.Join("\r\n", x.Value))));
        }

        private static string GetTypeRefFromNode(XmlNode node)
        {
            var typeName = node?.Attributes?["type"] == null ? "" : node.Attributes["type"].Value;
            if (!TypeHelper.LooksLikeTypeName(typeName))
                typeName = node?.Attributes?["ref"] == null ? "" : node.Attributes["ref"].Value;
            if (!TypeHelper.LooksLikeTypeName(typeName))
            {
                typeName = node?.InnerText;
            }

            if (!TypeHelper.LooksLikeTypeName(typeName))
                return null;
            if (string.IsNullOrWhiteSpace(typeName) || typeName.StartsWith("Sitecore") || typeName.StartsWith("System") ||
                typeName.StartsWith("Microsoft"))
                return null;

            return typeName;
        }

        private static XmlNodeList GetNodesToCheckForTypeRefs(string file)
        {
            var doc = new XmlDocument();
            doc.Load(file);

            var nodesWithTypes = doc.SelectNodes("//*[starts-with(@type,'Elision') or starts-with(@ref,'Elision') or name()='field']");
            return nodesWithTypes;
        }

        private static IEnumerable<string> GetAllConfigFiles()
        {
            var match = new Regex(@"\\App_Config\\", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var exclude = new Regex(@"\\(bin|obj)\\", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var files = Directory
                .GetFiles(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")), @"*.config", SearchOption.AllDirectories)
                .Where(x => match.IsMatch(x))
                .Where(x => !exclude.IsMatch(x))
                .ToArray();

            if (files.Length == 0)
                Assert.Inconclusive("No configuration files were found. Please confirm that this is correct.");
            return files;
        }
    }
}
