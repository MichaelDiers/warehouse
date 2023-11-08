namespace Warehouse.Api.Providers
{
    using MongoDB.Driver;
    using Warehouse.Api.Contracts.Config;
    using Warehouse.Api.Contracts.Database;
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
                .GetCollection<DatabaseStockItem>(DatabaseStockItem.CollectionName);
        }

        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item to be created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task CreateAsync(
            IStockItem stockItem,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            if (transactionHandle?.ClientSessionHandle is not null)
            {
                await this.stockItemCollection.InsertOneAsync(
                    transactionHandle.ClientSessionHandle,
                    new DatabaseStockItem(stockItem),
                    cancellationToken: cancellationToken);
            }
            else
            {
                await this.stockItemCollection.InsertOneAsync(
                    new DatabaseStockItem(stockItem),
                    cancellationToken: cancellationToken);
            }
        }

        /// <summary>
        ///     Deletes the specified stock item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result is true if the item is deleted and false otherwise.</returns>
        public async Task<bool> DeleteAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle
        )
        {
            DeleteResult deleteResult;
            if (transactionHandle is not null)
            {
                deleteResult = await this.stockItemCollection.DeleteOneAsync(
                    doc => doc.UserId == userId && doc.StockItemId == stockItemId,
                    cancellationToken);
            }
            else
            {
                deleteResult = await this.stockItemCollection.DeleteOneAsync(
                    doc => doc.UserId == userId && doc.StockItemId == stockItemId,
                    cancellationToken);
            }

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount == 1;
        }

        /// <summary>
        ///     Reads all stock items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>All stock items with the specified user id.</returns>
        public async Task<IEnumerable<IStockItem>> ReadAsync(
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            IAsyncCursor<DatabaseStockItem> result;
            if (transactionHandle?.ClientSessionHandle is not null)
            {
                result = await this.stockItemCollection.FindAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.UserId == userId,
                    cancellationToken: cancellationToken);
            }
            else
            {
                result = await this.stockItemCollection.FindAsync(
                    doc => doc.UserId == userId,
                    cancellationToken: cancellationToken);
            }

            return await StockItemProvider.ToStockItems(
                result,
                cancellationToken);
        }

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>The found stock item.</returns>
        public async Task<IStockItem?> ReadByIdAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            IAsyncCursor<DatabaseStockItem> result;
            if (transactionHandle?.ClientSessionHandle is not null)
            {
                result = await this.stockItemCollection.FindAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.UserId == userId && doc.StockItemId == stockItemId,
                    cancellationToken: cancellationToken);
            }
            else
            {
                result = await this.stockItemCollection.FindAsync(
                    doc => doc.UserId == userId && doc.StockItemId == stockItemId,
                    cancellationToken: cancellationToken);
            }

            return StockItemProvider.ToStockItem(await result.FirstOrDefaultAsync(cancellationToken));
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        public async Task<bool> UpdateAsync(
            IStockItem stockItem,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            UpdateResult result;
            if (transactionHandle?.ClientSessionHandle is not null)
            {
                result = await this.stockItemCollection.UpdateOneAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.StockItemId == stockItem.Id && doc.UserId == stockItem.UserId,
                    Builders<DatabaseStockItem>.Update.Set(
                            doc => doc.Name,
                            stockItem.Name)
                        .Set(
                            doc => doc.Quantity,
                            stockItem.Quantity)
                        .Set(
                            doc => doc.MinimumQuantity,
                            stockItem.MinimumQuantity),
                    cancellationToken: cancellationToken);
            }
            else
            {
                result = await this.stockItemCollection.UpdateOneAsync(
                    doc => doc.StockItemId == stockItem.Id && doc.UserId == stockItem.UserId,
                    Builders<DatabaseStockItem>.Update.Set(
                            doc => doc.Name,
                            stockItem.Name)
                        .Set(
                            doc => doc.Quantity,
                            stockItem.Quantity)
                        .Set(
                            doc => doc.MinimumQuantity,
                            stockItem.MinimumQuantity),
                    cancellationToken: cancellationToken);
            }

            return result.IsAcknowledged && result.MatchedCount == 1;
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="stockItemId">The stock item that is updated.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the updated stock item or null if no item is found.</returns>
        public async Task<IStockItem?> UpdateQuantityAsync(
            string userId,
            string stockItemId,
            int quantityDelta,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            var options =
                new FindOneAndUpdateOptions<DatabaseStockItem, DatabaseStockItem>
                {
                    ReturnDocument = ReturnDocument.After
                };

            DatabaseStockItem? result;
            if (transactionHandle?.ClientSessionHandle is not null)
            {
                result = await this.stockItemCollection.FindOneAndUpdateAsync<DatabaseStockItem>(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.StockItemId == stockItemId && doc.UserId == userId,
                    Builders<DatabaseStockItem>.Update.Inc(
                        doc => doc.Quantity,
                        quantityDelta),
                    options,
                    cancellationToken);
            }
            else
            {
                result = await this.stockItemCollection.FindOneAndUpdateAsync<DatabaseStockItem>(
                    doc => doc.StockItemId == stockItemId && doc.UserId == userId,
                    Builders<DatabaseStockItem>.Update.Inc(
                        doc => doc.Quantity,
                        quantityDelta),
                    options,
                    cancellationToken);
            }

            return StockItemProvider.ToStockItem(result);
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
                databaseStockItem.UserId is null ||
                databaseStockItem.MinimumQuantity is null)
            {
                // Todo
                throw new NotImplementedException();
            }

            return new StockItem(
                databaseStockItem.StockItemId,
                databaseStockItem.Name,
                databaseStockItem.Quantity.Value,
                databaseStockItem.MinimumQuantity.Value,
                databaseStockItem.UserId);
        }

        /// <summary>
        ///     Converts from <see cref="IEnumerable{T}" /> of <see cref="DatabaseStockItem" /> to <see cref="IEnumerable{T}" /> of
        ///     <see cref="IStockItem" />.
        /// </summary>
        /// <param name="cursor">The database cursor.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The converted stock items.</returns>
        private static async Task<IEnumerable<IStockItem>> ToStockItems(
            IAsyncCursor<DatabaseStockItem> cursor,
            CancellationToken cancellationToken
        )
        {
            var result = new List<IStockItem>();
            await cursor.ForEachAsync(
                databaseStockItem =>
                {
                    result.Add(StockItemProvider.ToStockItem(databaseStockItem) ?? throw new NotImplementedException());
                },
                cancellationToken);
            return result;
        }
    }
}
