namespace Warehouse.Api.Services.Atomic
{
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Models.StockItems;

    /// <inheritdoc cref="IAtomicStockItemService" />
    internal class AtomicStockItemService : IAtomicStockItemService
    {
        /// <summary>
        ///     The provider for stock items.
        /// </summary>
        private readonly IStockItemProvider provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicStockItemService" /> class.
        /// </summary>
        /// <param name="provider">The provider for stock items.</param>
        public AtomicStockItemService(IStockItemProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="createStockItem">The stock item to be created.</param>
        /// <param name="userId">The unique id of the user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result is the created stock item.</returns>
        public async Task<IStockItem> CreateAsync(
            ICreateStockItem createStockItem,
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var stockItem = new StockItem(
                Guid.NewGuid().ToString(),
                createStockItem.Name,
                createStockItem.Quantity,
                createStockItem.MinimumQuantity,
                userId);

            await this.provider.CreateAsync(
                stockItem,
                cancellationToken,
                transactionHandle);

            return stockItem;
        }

        /// <summary>
        ///     Deletes the specified stock item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result is true if the item is deleted and false otherwise.</returns>
        public Task<bool> DeleteAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return this.provider.DeleteAsync(
                userId,
                stockItemId,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Reads all stock items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>All stock items with the specified user id.</returns>
        public Task<IEnumerable<IStockItem>> ReadAsync(
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return this.provider.ReadAsync(
                userId,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>The found stock item.</returns>
        public Task<IStockItem?> ReadByIdAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return this.provider.ReadByIdAsync(
                userId,
                stockItemId,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="updateStockItem">The stock item that is updated.</param>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        public Task<bool> UpdateAsync(
            IUpdateStockItem updateStockItem,
            string userId,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle = null
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var stockItem = new StockItem(
                updateStockItem.Id,
                updateStockItem.Name,
                updateStockItem.Quantity,
                updateStockItem.MinimumQuantity,
                userId);

            return this.provider.UpdateAsync(
                stockItem,
                cancellationToken,
                transactionHandle);
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
            cancellationToken.ThrowIfCancellationRequested();

            if (quantityDelta == 0)
            {
                return await this.ReadByIdAsync(
                    userId,
                    stockItemId,
                    cancellationToken,
                    transactionHandle);
            }

            return await this.provider.UpdateQuantityAsync(
                userId,
                stockItemId,
                quantityDelta,
                cancellationToken,
                transactionHandle);
        }
    }
}
