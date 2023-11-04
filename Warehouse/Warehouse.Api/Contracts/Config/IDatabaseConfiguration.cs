namespace Warehouse.Api.Contracts.Config
{
    /// <summary>
    ///     Describes the database configuration.
    /// </summary>
    public interface IDatabaseConfiguration
    {
        /// <summary>
        ///     Gets the connection string.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        ///     Gets the name of the database.
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        ///     Gets the name of the shopping item collection.
        /// </summary>
        string ShoppingItemCollectionName { get; }

        /// <summary>
        ///     Gets the name of the stock item collection.
        /// </summary>
        string StockItemCollectionName { get; }
    }
}
