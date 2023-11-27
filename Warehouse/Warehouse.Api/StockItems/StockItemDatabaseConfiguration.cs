namespace Warehouse.Api.StockItems
{
    using Generic.Base.Api.MongoDb;

    public class StockItemDatabaseConfiguration : IDatabaseConfiguration
    {
        public string CollectionName { get; set; }
        public string DatabaseName { get; set; }
    }
}
