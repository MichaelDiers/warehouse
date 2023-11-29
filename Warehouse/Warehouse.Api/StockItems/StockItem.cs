namespace Warehouse.Api.StockItems
{
    using Generic.Base.Api.Database;

    /// <summary>
    ///     Describes a stock item.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Database.IUserBoundEntry" />
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
        ///     Gets the minimum required quantity of the item in stock.
        /// </summary>
        public int MinimumQuantity { get; }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the quantity of the item in stock.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        ///     Gets the id of the item.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Gets the unique identifier of the owner.
        /// </summary>
        public string UserId { get; }
    }
}
