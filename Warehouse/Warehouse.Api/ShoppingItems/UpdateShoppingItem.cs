namespace Warehouse.Api.ShoppingItems
{
    public class UpdateShoppingItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateShoppingItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        public UpdateShoppingItem(string name, int quantity)
        {
            this.Name = name;
            this.Quantity = quantity;
        }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the required quantity of the item.
        /// </summary>
        public int Quantity { get; set; }
    }
}
