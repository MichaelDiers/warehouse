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
        /// <returns>A <see cref="Task" /> whose result is the created stock item.</returns>
        Task<IStockItem> CreateAsync(ICreateStockItem createStockItem);
    }
}
