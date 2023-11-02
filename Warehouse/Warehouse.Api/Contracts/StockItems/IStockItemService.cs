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
        /// <returns>A <see cref="Task" /> whose result is the created stock item.</returns>
        Task<IStockItem> CreateAsync(ICreateStockItem createStockItem, string userId);

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
        Task<IStockItem> ReadByIdAsync(string userId, string stockItemId);
    }
}
