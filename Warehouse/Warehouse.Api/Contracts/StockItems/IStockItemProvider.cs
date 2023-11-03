namespace Warehouse.Api.Contracts.StockItems
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
    }
}
