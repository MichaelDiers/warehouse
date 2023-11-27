namespace Warehouse.Api.ShoppingItems
{
    using Generic.Base.Api.MongoDb;

    public class ShoppingItemDatabaseConfiguration : IDatabaseConfiguration
    {
        public string CollectionName { get; set; }
        public string DatabaseName { get; set; }
    }
}
