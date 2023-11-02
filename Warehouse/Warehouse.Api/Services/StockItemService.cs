namespace Warehouse.Api.Services
{
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Models.StockItems;

    /// <inheritdoc cref="IStockItemService" />
    internal class StockItemService : IStockItemService
    {
        /// <summary>
        ///     The provider for stock items.
        /// </summary>
        private readonly IStockItemProvider provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItemService" /> class.
        /// </summary>
        /// <param name="provider">The provider for stock items.</param>
        public StockItemService(IStockItemProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        ///     Creates the specified stock item.
        /// </summary>
        /// <param name="createStockItem">The stock item to be created.</param>
        /// <param name="userId">The unique id of the user.</param>
        /// <returns>A <see cref="Task" /> whose result is the created stock item.</returns>
        public async Task<IStockItem> CreateAsync(ICreateStockItem createStockItem, string userId)
        {
            var stockItem = new StockItem(
                Guid.NewGuid().ToString(),
                createStockItem.Name,
                createStockItem.Quantity,
                userId);

            await this.provider.CreateAsync(stockItem);

            return stockItem;
        }
    }
}
