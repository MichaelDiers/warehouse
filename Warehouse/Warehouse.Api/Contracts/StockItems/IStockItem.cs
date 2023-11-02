namespace Warehouse.Api.Contracts.StockItems
{
    /// <summary>
    ///     Describes a stock item.
    /// </summary>
    public interface IStockItem
    {
        /// <summary>
        ///     Gets the id of the item.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the quantity of the item in stock.
        /// </summary>
        int Quantity { get; }

        /// <summary>
        ///     Gets the unique identifier of the owner.
        /// </summary>
        string UserId { get; }
    }
}
