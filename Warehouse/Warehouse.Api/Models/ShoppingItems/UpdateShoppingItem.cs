namespace Warehouse.Api.Models.ShoppingItems
{
    using Warehouse.Api.Contracts.ShoppingItems;

    /// <inheritdoc cref="IUpdateShoppingItem" />
    public class UpdateShoppingItem : IUpdateShoppingItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateShoppingItem" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="stockItemId">The stock item identifier</param>
        public UpdateShoppingItem(
            string id,
            string name,
            int quantity,
            string stockItemId
        )
        {
            this.Id = id;
            this.Name = name;
            this.Quantity = quantity;
            this.StockItemId = stockItemId;
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
        ///     Gets or sets the required quantity of the item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the stock item identifier.
        /// </summary>
        public string StockItemId { get; set; }
    }
}
