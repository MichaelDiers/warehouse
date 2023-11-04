namespace Warehouse.Api.Contracts.ShoppingItems
{
    /// <summary>
    ///     Describes a shopping item.
    /// </summary>
    public interface IShoppingItem
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
        ///     Gets the required quantity.
        /// </summary>
        int Quantity { get; }

        /// <summary>
        ///     Gets the unique identifier of the owner.
        /// </summary>
        string UserId { get; }
    }
}
