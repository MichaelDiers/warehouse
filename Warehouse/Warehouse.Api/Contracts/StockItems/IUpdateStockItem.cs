namespace Warehouse.Api.Contracts.StockItems
{
    /// <summary>
    ///     Describes the request data for updating a stock item.
    /// </summary>
    public interface IUpdateStockItem
    {
        /// <summary>
        ///     Gets the id of the item.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the minimum required quantity of the item in stock.
        /// </summary>
        int MinimumQuantity { get; }

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
