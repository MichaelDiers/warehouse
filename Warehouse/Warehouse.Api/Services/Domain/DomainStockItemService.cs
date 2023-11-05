namespace Warehouse.Api.Services.Domain
{
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.StockItems;

    /// <summary>
    ///     The domain service for handling stock items.
    /// </summary>
    /// <seealso cref="Warehouse.Api.Contracts.StockItems.IStockItemService" />
    public class DomainStockItemService : IStockItemService
    {
        /// <summary>
        ///     The atomic stock item service.
        /// </summary>
        private readonly IAtomicStockItemService atomicStockItemService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DomainStockItemService" /> class.
        /// </summary>
        /// <param name="atomicStockItemService">The atomic stock item service.</param>
        public DomainStockItemService(IAtomicStockItemService atomicStockItemService)
        {
            this.atomicStockItemService = atomicStockItemService;
        }

        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="createStockItem">The stock item to be created.</param>
        /// <param name="userId">The unique id of the user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the created stock item.</returns>
        public Task<IStockItem> CreateAsync(
            ICreateStockItem createStockItem,
            string userId,
            CancellationToken cancellationToken
        )
        {
            return this.atomicStockItemService.CreateAsync(
                createStockItem,
                userId,
                cancellationToken);
        }

        /// <summary>
        ///     Deletes the specified stock item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result is true if the item is deleted and false otherwise.</returns>
        public Task<bool> DeleteAsync(string userId, string stockItemId, CancellationToken cancellationToken)
        {
            return this.atomicStockItemService.DeleteAsync(
                userId,
                stockItemId,
                cancellationToken);
        }

        /// <summary>
        ///     Reads all stock items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>All stock items with the specified user id.</returns>
        public Task<IEnumerable<IStockItem>> ReadAsync(string userId, CancellationToken cancellationToken)
        {
            return this.atomicStockItemService.ReadAsync(
                userId,
                cancellationToken);
        }

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The found stock item.</returns>
        public Task<IStockItem?> ReadByIdAsync(string userId, string stockItemId, CancellationToken cancellationToken)
        {
            return this.atomicStockItemService.ReadByIdAsync(
                userId,
                stockItemId,
                cancellationToken);
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="stockItemId">The stock item that is updated.</param>
        /// <param name="operation">Specifies the type of the update.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        public Task<bool> UpdateAsync(
            string userId,
            string stockItemId,
            UpdateOperation operation,
            int quantityDelta,
            CancellationToken cancellationToken
        )
        {
            return this.atomicStockItemService.UpdateAsync(
                userId,
                stockItemId,
                operation,
                quantityDelta,
                cancellationToken);
        }

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="updateStockItem">The stock item that is updated.</param>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        public Task<bool> UpdateAsync(
            IUpdateStockItem updateStockItem,
            string userId,
            CancellationToken cancellationToken
        )
        {
            return this.atomicStockItemService.UpdateAsync(
                updateStockItem,
                userId,
                cancellationToken);
        }
    }
}
