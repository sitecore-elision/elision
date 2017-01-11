using System;
using System.Collections.Generic;
using System.Linq;
using Elision.Foundation.Mvc.Pipelines.GetControllerRenderingValueParameters;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.Mvc.Presentation;

namespace Elision.Foundation.Mvc.Tests
{
    [TestFixture]
    public class AddRenderingParametersTests
    {
        private readonly GetControllerRenderingValueParametersArgs _resultArgs;
        private readonly RenderingContext _buildTestRenderingContext;

        public AddRenderingParametersTests()
        {
            using (var db = new Db
            {
                new DbItem("page"),
                new DbItem("datasourcefolder")
                {
                    new DbItem("dsitem1"),
                    new DbItem("dsitem2"),
                    new DbItem("dsitem3")
                }
            })
            {
                var processor = new AddRenderingParameters();

                _buildTestRenderingContext = BuildTestRenderingContext(db);
                _resultArgs = new GetControllerRenderingValueParametersArgs(null, _buildTestRenderingContext);

                processor.Process(_resultArgs);
            }
        }

        private RenderingContext BuildTestRenderingContext(Db db)
        {
            var dsItems = db.Database.SelectItems("/sitecore/content/datasourcefolder/*");

            var context = new RenderingContext
            {
                Rendering = new Rendering
                {
                    UniqueId = Guid.NewGuid(),
                    DataSource = "/sitecore/content/datasourcefolder/dsitem2",
                    Parameters = new RenderingParameters("param1=/sitecore/content/datasourcefolder/dsitem3&param2=1&param3=" + string.Join("|", dsItems.Select(x => x.ID.ToString())))
                },
                ContextItem = db.GetItem("/sitecore/content/page")
            };
            var pageContext = new PageContext();
            new PrivateObject(context).SetField("pageContext", pageContext);
            new PrivateObject(pageContext).SetField("item", db.GetItem("/sitecore/content/page"));

            return context;
        }

        [Test]
        public void AddsRenderingToParametersList()
        {
            var value = _resultArgs.Parameters["rendering"];
            value.Should().BeOfType<Rendering>();
        }

        [Test]
        public void AddsRenderingUniqueIdToParametersList()
        {
            var value = (ID) _resultArgs.Parameters["renderingUniqueId"];
            value.Should().Be(new ID(_buildTestRenderingContext.Rendering.UniqueId));
        }

        [Test]
        public void AddsPageContextToParametersList()
        {
            var value = (PageContext)_resultArgs.Parameters["pageContext"];
            value.Should().Be(_buildTestRenderingContext.PageContext);
        }

        [Test]
        public void AddsRenderingDatasourceToParametersList()
        {
            var value = (string)_resultArgs.Parameters["renderingDataSource"];
            value.Should().Be(_buildTestRenderingContext.Rendering.DataSource);
        }

        [Test]
        public void AddsRenderingContextItemToParametersList()
        {
            var value = _resultArgs.Parameters["renderingContextItem"];
            value.Should().Be(_buildTestRenderingContext.ContextItem);
        }

        [Test]
        public void AddsPageContextItemToParametersList()
        {
            var value = _resultArgs.Parameters["pageContextItem"];
            value.Should().Be(_buildTestRenderingContext.PageContext.Item);
        }

        [Test]
        public void AddsRenderingParameterToParametersList()
        {
            var value = (string)_resultArgs.Parameters["param2"];
            value.Should().Be("1");
        }

        [Test]
        public void AddsRenderingParameterItemToParametersList()
        {
            var value = (Item)_resultArgs.Parameters["param1Item"];
            value.Name.Should().Be("dsitem3");
        }

        [Test]
        public void AddsBoolRenderingParameterToParametersList()
        {
            var ids = _buildTestRenderingContext.Rendering.Parameters["param3"];
            var value = string.Join("|", ((IEnumerable<Item>)_resultArgs.Parameters["param3Items"]).Select(x => x.ID.ToString()));
            value.Should().Be(value);
        }
    }
}
