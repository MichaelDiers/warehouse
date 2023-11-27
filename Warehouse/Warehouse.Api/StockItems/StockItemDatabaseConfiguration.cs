namespace Warehouse.Api.StockItems
{
    using Generic.Base.Api.MongoDb;

    /// <summary>
    ///     Database configuration for stock items.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.MongoDb.IDatabaseConfiguration" />
    public class StockItemDatabaseConfiguration : IDatabaseConfiguration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItemDatabaseConfiguration" /> class.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="databaseName">Name of the database.</param>
        public StockItemDatabaseConfiguration(string collectionName, string databaseName)
        {
            this.CollectionName = collectionName;
            this.DatabaseName = databaseName;
        }

        /// <summary>
        ///     Gets the name of the collection.
        /// </summary>
        /// <value>
        ///     The name of the collection.
        /// </value>
        public string CollectionName { get; }

        /// <summary>
        ///     Gets the name of the database.
        /// </summary>
        /// <value>
        ///     The name of the database.
        /// </value>
        public string DatabaseName { get; }
    }
}
