namespace Warehouse.Api.Providers
{
    using MongoDB.Driver;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Models.StockItems;

    /// <inheritdoc cref="IStockItemProvider" />
    internal class StockItemProvider : BaseProvider, IStockItemProvider
    {
        /// <summary>
        ///     The collection that contains stock items.
        /// </summary>
        private readonly IMongoCollection<DatabaseStockItem> stockItemCollection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItemProvider" /> class.
        /// </summary>
        /// <param name="transactionHandler">A transaction handler.</param>
        /// <param name="database">The mongo database.</param>
        public StockItemProvider(ITransactionHandler transactionHandler, IMongoDatabase database)
            : base(
                nameof(StockItemProvider),
                transactionHandler)
        {
            this.stockItemCollection = database.GetCollection<DatabaseStockItem>(DatabaseStockItem.CollectionName);
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
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.stockItemCollection.InsertOneAsync(
                    transactionHandle.ClientSessionHandle,
                    new DatabaseStockItem(stockItem),
                    cancellationToken: cancellationToken));
        }

        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item to be created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task CreateAsync(IStockItem stockItem, CancellationToken cancellationToken)
        {
            await this.Execute(
                session => this.CreateAsync(
                    stockItem,
                    cancellationToken,
                    session),
                cancellationToken);
        }

        /// <summary>
        ///     Deletes the specified stock item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(string userId, string stockItemId, CancellationToken cancellationToken)
        {
            await this.Execute(
                session => this.DeleteAsync(
                    userId,
                    stockItemId,
                    cancellationToken,
                    session),
                cancellationToken);
        }

        /// <summary>
        ///     Deletes the specified stock item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.stockItemCollection.DeleteOneAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.UserId == userId && doc.StockItemId == stockItemId,
                    cancellationToken: cancellationToken));
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
            ITransactionHandle transactionHandle
        )
        {
            var result = await this.Execute(
                () => this.stockItemCollection.FindAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.UserId == userId,
                    cancellationToken: cancellationToken));

            return await StockItemProvider.ToStockItems(
                result,
                cancellationToken);
        }

        /// <summary>
        ///     Reads all stock items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>All stock items with the specified user id.</returns>
        public async Task<IEnumerable<IStockItem>> ReadAsync(string userId, CancellationToken cancellationToken)
        {
            return await this.Execute(
                session => this.ReadAsync(
                    userId,
                    cancellationToken,
                    session),
                cancellationToken);
        }

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The found stock item.</returns>
        public async Task<IStockItem> ReadByIdAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken
        )
        {
            return await this.Execute(
                session => this.ReadByIdAsync(
                    userId,
                    stockItemId,
                    cancellationToken,
                    session),
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
        public async Task<IStockItem> ReadByIdAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            return await this.Execute(
                () => this.stockItemCollection.FindAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.UserId == userId && doc.StockItemId == stockItemId,
                    cancellationToken: cancellationToken),
                StockItemProvider.ToStockItem,
                cancellationToken);
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(IStockItem stockItem, CancellationToken cancellationToken)
        {
            await this.Execute(
                session => this.UpdateAsync(
                    stockItem,
                    cancellationToken,
                    session),
                cancellationToken);
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(
            IStockItem stockItem,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.stockItemCollection.UpdateOneAsync(
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
                    cancellationToken: cancellationToken));
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="stockItemId">The stock item that is updated.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the updated stock item.</returns>
        public async Task<IStockItem> UpdateQuantityAsync(
            string userId,
            string stockItemId,
            int quantityDelta,
            CancellationToken cancellationToken
        )
        {
            return await this.Execute(
                session => this.UpdateQuantityAsync(
                    userId,
                    stockItemId,
                    quantityDelta,
                    cancellationToken,
                    session),
                cancellationToken);
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="stockItemId">The stock item that is updated.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the updated stock item.</returns>
        public async Task<IStockItem> UpdateQuantityAsync(
            string userId,
            string stockItemId,
            int quantityDelta,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            var options =
                new FindOneAndUpdateOptions<DatabaseStockItem, DatabaseStockItem>
                {
                    ReturnDocument = ReturnDocument.After
                };

            return await this.Execute(
                () => this.stockItemCollection.FindOneAndUpdateAsync<DatabaseStockItem>(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.StockItemId == stockItemId && doc.UserId == userId,
                    Builders<DatabaseStockItem>.Update.Inc(
                        doc => doc.Quantity,
                        quantityDelta),
                    options,
                    cancellationToken),
                StockItemProvider.ToStockItem);
        }

        /// <summary>
        ///     Converts from <see cref="DatabaseStockItem" /> to <see cref="IStockItem" />.
        /// </summary>
        /// <param name="databaseStockItem">The database stock item.</param>
        /// <returns>The converted stock item.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private static IStockItem ToStockItem(DatabaseStockItem databaseStockItem)
        {
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
