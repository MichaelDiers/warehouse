namespace Warehouse.Api.Models.StockItems
{
    using Warehouse.Api.Contracts.StockItems;

    /// <inheritdoc cref="IStockItem" />
    public class StockItem : IStockItem
    {
        /// <summary>
        ///     Gets or sets the id of the item.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the quantity of the item in stock.
        /// </summary>
        public int Quantity { get; set; }
    }
}
