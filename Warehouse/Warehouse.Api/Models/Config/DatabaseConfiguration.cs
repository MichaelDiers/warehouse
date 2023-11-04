namespace Warehouse.Api.Models.Config
{
    using Warehouse.Api.Contracts.Config;

    /// <inheritdoc cref="IDatabaseConfiguration" />
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the name of the database.
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        ///     Gets the name of the shopping item collection.
        /// </summary>
        public string ShoppingItemCollectionName { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the name of the stock item collection.
        /// </summary>
        public string StockItemCollectionName { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the connection string.
        /// </summary>
        string IDatabaseConfiguration.ConnectionString => this.ConnectionString;

        /// <summary>
        ///     Gets the name of the database.
        /// </summary>
        string IDatabaseConfiguration.DatabaseName => this.DatabaseName;

        /// <summary>
        ///     Gets or sets the name of the shopping item collection.
        /// </summary>
        string IDatabaseConfiguration.ShoppingItemCollectionName => this.ShoppingItemCollectionName;

        /// <summary>
        ///     Gets or sets the name of the stock item collection.
        /// </summary>
        string IDatabaseConfiguration.StockItemCollectionName => this.StockItemCollectionName;
    }
}
