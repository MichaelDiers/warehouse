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
    }
}
