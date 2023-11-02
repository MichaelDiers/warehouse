namespace Warehouse.Api.Contracts.StockItems
{
    /// <summary>
    ///     Describes the request data for creating a stock item.
    /// </summary>
    public interface ICreateStockItem
    {
        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the quantity of the item in stock.
        /// </summary>
        int Quantity { get; }
    }
}
