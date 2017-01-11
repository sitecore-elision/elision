using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Elision.Foundation.Kernel.Tests
{
    [TestFixture]
    public class ResolveDatasourceTests
    {
        private readonly Dictionary<string, ID> _datasourceItems = new Dictionary<string, ID>
        {
            {"ds1", ID.NewID},
            {"ds2", ID.NewID}
        };

        [Test]
        public void ResolvesItemFromId()
        {
            var ds = $"{_datasourceItems["ds1"]}";

            using (var db = GetDb())
            {
                var result = db.Database.ResolveDatasource(ds);
                result.Name.Should().Be("ds1");
            }
        }

        [Test]
        public void ResolvesItemFromShortId()
        {
            var ds = $"{_datasourceItems["ds1"].ToShortID()}";

            using (var db = GetDb())
            {
                var result = db.Database.ResolveDatasource(ds);
                result.Name.Should().Be("ds1");
            }
        }

        [Test]
        public void ResolvesItemsFromIds()
        {
            var ds = $"{_datasourceItems["ds1"]}|{_datasourceItems["ds2"]}";

            using (var db = GetDb())
            {
                var results = db.Database.ResolveDatasourceItems(ds).ToArray();
                results[0].Name.Should().Be("ds1");
                results[1].Name.Should().Be("ds2");
            }
        }

        [Test]
        public void ResolvesFromAbsoluteQuery()
        {
            var ds = "query:/sitecore/content/*";
            using (var db = GetDb())
            {
                var results = db.Database.ResolveDatasourceItems(ds).ToArray();
                results[0].Name.Should().Be("ds1");
                results[1].Name.Should().Be("ds2");
            }
        }

        [Test]
        public void ResolvesFromAbsoluteQueryWithoutQueryPrefix()
        {
            var ds = "/sitecore/content/*";
            using (var db = GetDb())
            {
                var results = db.Database.ResolveDatasourceItems(ds).ToArray();
                results[0].Name.Should().Be("ds1");
                results[1].Name.Should().Be("ds2");
            }
        }

        [Test]
        public void ResolvesFromRelativeQuery()
        {
            using (var db = GetDb())
            {
                var ds = "query:./*";
                var contextItem = db.GetItem("/sitecore/content");
                var results = db.Database.ResolveDatasourceItems(ds, contextItem).ToArray();
                results[0].Name.Should().Be("ds1");
                results[1].Name.Should().Be("ds2");
            }
        }

        [Test]
        public void ResolvesFromRelativeQueryWithoutQueryPrefix()
        {
            using (var db = GetDb())
            {
                var ds = "./*";
                var contextItem = db.GetItem("/sitecore/content");
                var results = db.Database.ResolveDatasourceItems(ds, contextItem).ToArray();
                results[0].Name.Should().Be("ds1");
                results[1].Name.Should().Be("ds2");
            }
        }

        protected Db GetDb()
        {
            var db = new Db();

            foreach (var item in _datasourceItems)
            {
                db.Add(new DbItem(item.Key, item.Value));
            }

            return db;
        }
    }
}
