namespace Warehouse.Api.Contracts.StockItems
{
    using Warehouse.Api.Contracts.Database;

    /// <summary>
    ///     The provider for handling stock items.
    /// </summary>
    public interface IStockItemProvider
    {
        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item to be created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task CreateAsync(
            IStockItem stockItem,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );

        /// <summary>
        ///     Deletes the specified stock item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task DeleteAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );

        /// <summary>
        ///     Reads all stock items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>All stock items with the specified user id.</returns>
        Task<IEnumerable<IStockItem>> ReadAsync(string userId, CancellationToken cancellationToken);

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The found stock item.</returns>
        Task<IStockItem> ReadByIdAsync(string userId, string stockItemId, CancellationToken cancellationToken);

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">A handle for transactions.</param>
        /// <returns>The found stock item.</returns>
        Task<IStockItem> ReadByIdAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateAsync(
            IStockItem stockItem,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="stockItemId">The stock item that is updated.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the updated stock item.</returns>
        Task<IStockItem> UpdateQuantityAsync(
            string userId,
            string stockItemId,
            int quantityDelta,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );
    }
}
