using System.Web.Mvc;
using Elision.Ioc.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Sitecore.Pipelines;

namespace Elision.Ioc.Tests
{
    [TestFixture]
    public class InitializeControllerFactoryTests
    {
        [Test]
        public void ReplacesDefaultController()
        {
            var processor = new InitializeControllerFactory();

            processor.Process(new PipelineArgs());

            ControllerBuilder.Current.GetControllerFactory().Should().BeOfType<ElisionControllerFactory>();
        }

        [Test]
        public void StoresDefaultFactoryAsFallback()
        {
            var defaultFactory = ControllerBuilder.Current.GetControllerFactory();

            var processor = new InitializeControllerFactory();

            processor.Process(new PipelineArgs());

            var currentFactory = ControllerBuilder.Current.GetControllerFactory();
            currentFactory.Should().BeOfType<ElisionControllerFactory>();

            var privateFactory = new PrivateObject(currentFactory);
            var innerFactory = privateFactory.GetField("_innerFactory");
            innerFactory.Should().NotBe(currentFactory);
            innerFactory.Should().Be(defaultFactory);
        }
    }
}
