namespace Warehouse.Api.Providers
{
    using Warehouse.Api.Contracts.StockItems;

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
    }
}
