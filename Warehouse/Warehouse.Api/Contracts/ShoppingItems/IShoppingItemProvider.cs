namespace Warehouse.Api.Contracts.ShoppingItems
{
    using Warehouse.Api.Contracts.Database;

    /// <summary>
    ///     The provider for handling shopping items.
    /// </summary>
    public interface IShoppingItemProvider
    {
        /// <summary>
        ///     Creates the specified shopping item.
        /// </summary>
        /// <param name="shoppingItem">The shopping item to be created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task CreateAsync(
            IShoppingItem shoppingItem,
            CancellationToken cancellationToken,
            ITransactionHandle? transactionHandle
        );

        /// <summary>
        ///     Deletes the specified shopping item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result is true if the item is deleted and false otherwise.</returns>
        Task<bool> DeleteAsync(string userId, string shoppingItemId, CancellationToken cancellationToken);

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
        Task<IShoppingItem?> ReadByIdAsync(string userId, string shoppingItemId, CancellationToken cancellationToken);

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="shoppingItemId">The shopping item that is updated.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        Task<bool> UpdateAsync(
            string userId,
            string shoppingItemId,
            int quantityDelta,
            CancellationToken cancellationToken
        );

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="shoppingItem">The shopping item that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        Task<bool> UpdateAsync(IShoppingItem shoppingItem, CancellationToken cancellationToken);
    }
}
