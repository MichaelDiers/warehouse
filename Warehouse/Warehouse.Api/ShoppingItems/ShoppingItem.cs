namespace Warehouse.Api.ShoppingItems
{
    using Generic.Base.Api.Database;

    public class ShoppingItem : IUserBoundEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShoppingItem" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="userId">The user identifier.</param>
        public ShoppingItem(
            string id,
            string name,
            int quantity,
            string userId
        )
        {
            this.Id = id;
            this.Name = name;
            this.Quantity = quantity;
            this.UserId = userId;
        }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the required quantity of the item.
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
