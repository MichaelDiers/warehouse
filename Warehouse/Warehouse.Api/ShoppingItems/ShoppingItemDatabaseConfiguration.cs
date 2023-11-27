namespace Warehouse.Api.ShoppingItems
{
    using Generic.Base.Api.MongoDb;

    /// <summary>
    ///     Database configuration for shopping items.
    /// </summary>
    /// <seealso cref="IDatabaseConfiguration" />
    public class ShoppingItemDatabaseConfiguration : IDatabaseConfiguration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShoppingItemDatabaseConfiguration" /> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="databaseName">Name of the database.</param>
        public ShoppingItemDatabaseConfiguration(string collectionName, string databaseName)
        {
            this.CollectionName = collectionName;
            this.DatabaseName = databaseName;
        }

        /// <summary>
        ///     Gets the name of the collection.
        /// </summary>
        public string CollectionName { get; }

        /// <summary>
        ///     Gets the name of the database.
        /// </summary>
        public string DatabaseName { get; }
    }
}
