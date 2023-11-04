namespace Warehouse.Api.Contracts.ShoppingItems
{
    /// <summary>
    ///     Describes the request data for creating a shopping item.
    /// </summary>
    public interface ICreateShoppingItem
    {
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
