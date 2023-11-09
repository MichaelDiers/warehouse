namespace Warehouse.Api.Contracts.ShoppingItems
{
    /// <summary>
    ///     The business logic for handling shopping items.
    /// </summary>
    public interface IDomainShoppingItemService
    {
        /// <summary>
        ///     Reads all shopping items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>All shopping items with the specified user id.</returns>
        Task<IEnumerable<IShoppingItem>> ReadAsync(string userId, CancellationToken cancellationToken);

        /// <summary>
        ///     Reads a shopping item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The found shopping item.</returns>
        Task<IShoppingItem> ReadByIdAsync(string userId, string shoppingItemId, CancellationToken cancellationToken);
    }
}
