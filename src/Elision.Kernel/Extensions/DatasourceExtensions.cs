using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Elision
{
    public static class DatasourceExtensions
    {
        public static Item ResolveDatasource(this Database db, string datasource, Item contextItem = null)
        {
            var items = ResolveDatasourceItems(db, datasource, contextItem);
            return items.FirstOrDefault();
        }

        public static IEnumerable<Item> ResolveDatasourceItems(this Database db, string datasource, Item contextItem = null)
        {
            if (string.IsNullOrWhiteSpace(datasource)) return new Item[0];

            try
            {
                var ids = ID.ParseArray(datasource);
                if (ids.Length > 0)
                    return ids.Select(db.GetItem).Where(x => x != null);
            }
            catch { }

            var itemId = ShortID.IsShortID(datasource)
                ? ShortID.Parse(datasource).ToID()
                : ID.Null;

            if (!ID.IsNullOrEmpty(itemId))
            {
                var item = db.GetItem(itemId);
                return item == null
                           ? new Item[0]
                           : new[] {item};
            }

            string query = null;
            if (!string.IsNullOrWhiteSpace(datasource) && datasource.StartsWith("query:"))
                query = datasource.Substring("query:".Length);
            else if (!string.IsNullOrWhiteSpace(datasource) && (datasource.StartsWith("/") || datasource.StartsWith("./") || datasource.StartsWith("../")))
                query = datasource;

            if (!string.IsNullOrWhiteSpace(query))
                return contextItem == null
                           ? db.SelectItems(query)
                           : contextItem.Axes.SelectItems(query);
            return new Item[0];
        }

        public static IEnumerable<Item> GetLinkedItems(this Item item, string fieldName)
        {
            return item?.Database?.ResolveDatasourceItems(item[fieldName], item);
        }

        public static IEnumerable<Item> GetLinkedItems(this Item item, ID fieldId)
        {
            return item?.Database?.ResolveDatasourceItems(item[fieldId], item);
        }
    }
}
