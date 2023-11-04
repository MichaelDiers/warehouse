namespace Warehouse.Api.Contracts.ShoppingItems
{
    /// <summary>
    ///     Describes the request data for updating a shopping item.
    /// </summary>
    public interface IUpdateShoppingItem
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
    }
}
