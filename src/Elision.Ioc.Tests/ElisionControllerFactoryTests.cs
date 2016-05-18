using System.Collections.Generic;
using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;
using Elision.Ioc.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Elision.Ioc.Tests
{
    [TestFixture]
    public class ElisionControllerFactoryTests
    {
        public static XmlNode ToXmlNode(XElement element, XmlDocument xmlDoc = null)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                if (xmlDoc == null) xmlDoc = new XmlDocument();
                return xmlDoc.ReadNode(xmlReader);
            }
        }

        [Test]
        public void CanResolveControllerFromConfig()
        {
            var factory = new ElisionControllerFactory(null);
            var privateFactory = new PrivateObject(factory);

            var knownTypes = (Dictionary<string, XmlNode>)privateFactory.GetField("_knownTypes");
            var xElement = XElement.Parse($@"<controller type=""{typeof(TestTypesForIocTesting.TestController).AssemblyQualifiedName}"" />");
            knownTypes.Add("controllers/test", ToXmlNode(xElement));

            var controller = factory.CreateController(new RequestContext(), "controllers/test");

            controller.Should().BeOfType<TestTypesForIocTesting.TestController>();
        }

        [Test]
        public void CanResolveControllerFromTypeName()
        {
            var factory = new ElisionControllerFactory(null);
            factory.CreateController(new RequestContext(), typeof (TestTypesForIocTesting.TestController).AssemblyQualifiedName).Should().NotBeNull();
        }
    }
}
