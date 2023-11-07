namespace Warehouse.Api.Models.StockItems
{
    using System.Text.Json.Serialization;
    using Warehouse.Api.Contracts.StockItems;

    /// <inheritdoc cref="IStockItem" />
    public class StockItem : IStockItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItem" /> class.
        /// </summary>
        [JsonConstructor]
        public StockItem()
            : this(
                string.Empty,
                string.Empty,
                0,
                0,
                string.Empty)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItem" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="minimumQuantity">The minimal required quantity.</param>
        /// <param name="userId">The user identifier.</param>
        public StockItem(
            string id,
            string name,
            int quantity,
            int minimumQuantity,
            string userId
        )
        {
            this.Id = id;
            this.Name = name;
            this.Quantity = quantity;
            this.MinimumQuantity = minimumQuantity;
            this.UserId = userId;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItem" /> class.
        /// </summary>
        /// <param name="stockItem">The stock item that is copied.</param>
        public StockItem(IStockItem stockItem)
            : this(
                stockItem.Id,
                stockItem.Name,
                stockItem.Quantity,
                stockItem.MinimumQuantity,
                stockItem.UserId)
        {
        }

        /// <summary>
        ///     Gets or sets the id of the item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the minimum required quantity of the item in stock.
        /// </summary>
        public int MinimumQuantity { get; set; }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the quantity of the item in stock.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier of the owner.
        /// </summary>
        public string UserId { get; set; }
    }
}
