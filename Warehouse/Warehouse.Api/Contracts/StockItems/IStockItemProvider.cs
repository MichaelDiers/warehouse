﻿namespace Warehouse.Api.Contracts.StockItems
{
    /// <summary>
    ///     The provider for handling stock items.
    /// </summary>
    public interface IStockItemProvider
    {
        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item to be created.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task CreateAsync(IStockItem stockItem);

        /// <summary>
        ///     Deletes the specified stock item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result is true if the item is deleted and false otherwise.</returns>
        Task<bool> DeleteAsync(string userId, string stockItemId);

        /// <summary>
        ///     Reads all stock items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>All stock items with the specified user id.</returns>
        Task<IEnumerable<IStockItem>> ReadAsync(string userId);

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <returns>The found stock item.</returns>
        Task<IStockItem?> ReadByIdAsync(string userId, string stockItemId);

        /// <summary>
        ///     Updates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item that is updated.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        Task<bool> UpdateAsync(IStockItem stockItem);
    }
}
