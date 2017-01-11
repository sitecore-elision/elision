using Elision.Foundation.LookupSourceItems.Pipelines.ReplaceLookupSourceQueryTokens;
using NUnit.Framework;
using Sitecore.FakeDb;

namespace Elision.Foundation.LookupSourceItems.Tests
{
    [TestFixture]
    public class ReplaceItemFieldValueTokensTests
    {
        [TestCase("{ItemField:AuthorName}", ExpectedResult = "John Smith")]
        [TestCase("{ItemField:Author Full Name}", ExpectedResult = "John L. Smith")]
        public string ReplacesStringFieldValue(string query)
        {
            using (var db = GetFakeDb())
            {
                var item = db.GetItem("/sitecore/content/page");

                var processor = new ReplaceItemFieldValueTokens();
                var args = new ReplaceLookupSourceQueryTokensArgs(item, query);
                processor.Process(args);

                return args.Query;
            }
        }

        private Db GetFakeDb()
        {
            return new Db
            {
                new DbItem("page")
                {
                    new DbField("AuthorName") {Value = "John Smith"},
                    new DbField("Author Full Name") {Value = "John L. Smith"}
                }
            };
        }
    }
}
