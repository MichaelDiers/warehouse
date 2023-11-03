namespace Warehouse.Api.Contracts.StockItems
{
    /// <summary>
    ///     The business logic for handling stock items.
    /// </summary>
    public interface IStockItemService
    {
        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="createStockItem">The stock item to be created.</param>
        /// <param name="userId">The unique id of the user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the created stock item.</returns>
        Task<IStockItem> CreateAsync(
            ICreateStockItem createStockItem,
            string userId,
            CancellationToken cancellationToken
        );

        /// <summary>
        ///     Deletes the specified stock item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result is true if the item is deleted and false otherwise.</returns>
        Task<bool> DeleteAsync(string userId, string stockItemId, CancellationToken cancellationToken);

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
        Task<IStockItem?> ReadByIdAsync(string userId, string stockItemId, CancellationToken cancellationToken);

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="updateStockItem">The stock item that is updated.</param>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        Task<bool> UpdateAsync(IUpdateStockItem updateStockItem, string userId, CancellationToken cancellationToken);
    }
}
