namespace Warehouse.Api.StockItems
{
    using Generic.Base.Api.Database;

    public class StockItem : IUserBoundEntry
    {
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
        ///     Gets or sets the id of the item.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the unique identifier of the owner.
        /// </summary>
        public string UserId { get; set; }
    }
}
