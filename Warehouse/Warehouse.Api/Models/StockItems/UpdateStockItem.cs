namespace Warehouse.Api.Models.StockItems
{
    using Warehouse.Api.Contracts.StockItems;

    /// <inheritdoc cref="IUpdateStockItem" />
    public class UpdateStockItem : IUpdateStockItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateStockItem" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        public UpdateStockItem(string id, string name, int quantity)
        {
            this.Id = id;
            this.Name = name;
            this.Quantity = quantity;
        }

        /// <summary>
        ///     Gets or sets the id of the item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the quantity of the item in stock.
        /// </summary>
        public int Quantity { get; set; }
    }
}
