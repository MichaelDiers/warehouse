namespace Warehouse.Api.Providers
{
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Models.StockItems;

    /// <inheritdoc cref="IStockItemProvider" />
    internal class StockItemProvider : IStockItemProvider
    {
        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="stockItem">The stock item to be created.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public Task CreateAsync(IStockItem stockItem)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Reads all stock items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>All stock items with the specified user id.</returns>
        public Task<IEnumerable<IStockItem>> ReadAsync(string userId)
        {
            return Task.FromResult(Enumerable.Empty<IStockItem>());
        }

        /// <summary>
        ///     Reads a stock item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <returns>The found stock item.</returns>
        public Task<IStockItem> ReadByIdAsync(string userId, string stockItemId)
        {
            return Task.FromResult<IStockItem>(
                new StockItem(
                    stockItemId,
                    "name",
                    20,
                    userId));
        }
    }
}
