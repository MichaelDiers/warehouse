namespace Warehouse.Api.Providers
{
    using MongoDB.Driver;
    using Warehouse.Api.Contracts.Config;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Models.StockItems;

    /// <inheritdoc cref="IStockItemProvider" />
    internal class StockItemProvider : IStockItemProvider
    {
        /// <summary>
        ///     The collection that contains stock items.
        /// </summary>
        private readonly IMongoCollection<DatabaseStockItem> stockItemCollection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItemProvider" /> class.
        /// </summary>
        /// <param name="mongoClient">The mongo client.</param>
        /// <param name="configuration">The configuration.</param>
        public StockItemProvider(IMongoClient mongoClient, IAppConfiguration configuration)
        {
            this.stockItemCollection = mongoClient.GetDatabase(configuration.Warehouse.DatabaseName)
                .GetCollection<DatabaseStockItem>(configuration.Warehouse.StockItemCollectionName);
        }

        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item to be created.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task CreateAsync(IStockItem stockItem)
        {
            await this.stockItemCollection.InsertOneAsync(new DatabaseStockItem(stockItem));
        }

        /// <summary>
        ///     Reads all stock items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>All stock items with the specified user id.</returns>
        public async Task<IEnumerable<IStockItem>> ReadAsync(string userId)
        {
            var result = await this.stockItemCollection.FindAsync(doc => doc.UserId == userId);
            return await StockItemProvider.ToStockItems(result);
        }

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <returns>The found stock item.</returns>
        public async Task<IStockItem?> ReadByIdAsync(string userId, string stockItemId)
        {
            var result =
                await this.stockItemCollection.FindAsync(doc => doc.UserId == userId && doc.StockItemId == stockItemId);
            return StockItemProvider.ToStockItem(await result.FirstOrDefaultAsync());
        }

        /// <summary>
        ///     Converts from <see cref="DatabaseStockItem" /> to <see cref="IStockItem" />.
        /// </summary>
        /// <param name="databaseStockItem">The database stock item.</param>
        /// <returns>The converted stock item.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private static IStockItem? ToStockItem(DatabaseStockItem? databaseStockItem)
        {
            if (databaseStockItem is null)
            {
                return null;
            }

            if (databaseStockItem.Name is null ||
                databaseStockItem.Quantity is null ||
                databaseStockItem.StockItemId is null ||
                databaseStockItem.UserId is null)
            {
                // Todo
                throw new NotImplementedException();
            }

            return new StockItem(
                databaseStockItem.StockItemId,
                databaseStockItem.Name,
                databaseStockItem.Quantity.Value,
                databaseStockItem.UserId);
        }

        /// <summary>
        ///     Converts from <see cref="IEnumerable{T}" /> of <see cref="DatabaseStockItem" /> to <see cref="IEnumerable{T}" /> of
        ///     <see cref="IStockItem" />.
        /// </summary>
        /// <param name="cursor">The database cursor.</param>
        /// <returns>The converted stock items.</returns>
        private static async Task<IEnumerable<IStockItem>> ToStockItems(IAsyncCursor<DatabaseStockItem> cursor)
        {
            var result = new List<IStockItem>();
            await cursor.ForEachAsync(
                databaseStockItem =>
                {
                    result.Add(StockItemProvider.ToStockItem(databaseStockItem) ?? throw new NotImplementedException());
                });
            return result;
        }
    }
}
