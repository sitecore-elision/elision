using System;
using Elision.Foundation.FieldTokens.Pipelines.ReplaceFieldValueTokens;
using FluentAssertions;
using NUnit.Framework;
using Sitecore;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Elision.Foundation.FieldTokens.Tests
{
    [TestFixture]
    public class ReplaceItemTokensTests
    {
        [Test]
        public void RetrievesCreatedValue()
        {
            using (var db = GetFakeDb())
            {
                var processor = new ReplaceItemTokens();
                var args = new ReplaceFieldValueTokensArgs("{Created:yyyy-MM-dd}", db.GetItem("/sitecore/content/page"));
                processor.Process(args);
                args.FieldValue.Should().Be(@"2015-01-01");
            }
        }

        [Test]
        public void ReplacesStringFieldValue()
        {
            using (var db = GetFakeDb())
            {
                var processor = new ReplaceItemTokens();
                var args = new ReplaceFieldValueTokensArgs("{ItemField:ATextField}", db.GetItem("/sitecore/content/page"));
                processor.Process(args);
                args.FieldValue.Should().Be(@"A text value");
            }
        }

        [Test]
        public void ReplacesDateTimeFieldValue()
        {
            using (var db = GetFakeDb())
            {
                var processor = new ReplaceItemTokens();
                var args = new ReplaceFieldValueTokensArgs("{ItemField:ADateField:yyyy-MM-dd}", db.GetItem("/sitecore/content/page"));
                processor.Process(args);
                args.FieldValue.Should().Be(@"2015-03-03");
            }
        }

        [Test]
        public void RetrievesCreatedByValue()
        {
            using (var db = GetFakeDb())
            {
                var processor = new ReplaceItemTokens();
                var args = new ReplaceFieldValueTokensArgs("{CreatedBy}", db.GetItem("/sitecore/content/page"));
                processor.Process(args);
                args.FieldValue.Should().Be(@"sitecore\creator");
            }
        }

        [Test]
        public void RetrievesUpdatedValue()
        {
            using (var db = GetFakeDb())
            {
                var processor = new ReplaceItemTokens();
                var args = new ReplaceFieldValueTokensArgs("{Updated:yyyy-MM-dd}", db.GetItem("/sitecore/content/page"));
                processor.Process(args);
                args.FieldValue.Should().Be(@"2015-02-02");
            }
        }

        [Test]
        public void RetrievesUpdatedByValue()
        {
            using (var db = GetFakeDb())
            {
                var processor = new ReplaceItemTokens();
                var args = new ReplaceFieldValueTokensArgs("{UpdatedBy}", db.GetItem("/sitecore/content/page"));
                processor.Process(args);
                args.FieldValue.Should().Be(@"sitecore\updater");
            }
        }

        private Db GetFakeDb()
        {
            return new Db()
            {
                new DbItem("page")
                {
                    new DbField("ATextField") { Value = "A text value"},
                    new DbField("ADateField") { Value = DateUtil.ToIsoDate(new DateTime(2015, 3, 3, 3, 3, 3))},
                    new DbField("__Created", FieldIDs.Created) { Value = DateUtil.ToIsoDate(new DateTime(2015, 1, 1, 1, 1, 1))},
                    new DbField("__Created by", FieldIDs.CreatedBy) {Value = @"sitecore\creator"},
                    new DbField("__Revision", FieldIDs.Revision) {Value = ID.NewID.ToString()},
                    new DbField("__Updated", FieldIDs.Updated) { Value = DateUtil.ToIsoDate(new DateTime(2015, 2, 2, 2, 2, 2))},
                    new DbField("__Updated by", FieldIDs.UpdatedBy) {Value = @"sitecore\updater"}
                }
            };
        }
    }
}
