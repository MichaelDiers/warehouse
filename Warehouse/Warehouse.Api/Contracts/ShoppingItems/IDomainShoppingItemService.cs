﻿namespace Warehouse.Api.Contracts.ShoppingItems
{
    /// <summary>
    ///     The business logic for handling shopping items.
    /// </summary>
    public interface IDomainShoppingItemService
    {
        /// <summary>
        ///     Creates the specified shopping item.
        /// </summary>
        /// <param name="createShoppingItem">The shopping item to be created.</param>
        /// <param name="userId">The unique id of the user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the created shopping item.</returns>
        Task<IShoppingItem> CreateAsync(
            ICreateShoppingItem createShoppingItem,
            string userId,
            CancellationToken cancellationToken
        );

        /// <summary>
        ///     Deletes the specified shopping item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task DeleteAsync(string userId, string shoppingItemId, CancellationToken cancellationToken);

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

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="updateShoppingItem">The shopping item that is updated.</param>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateAsync(IUpdateShoppingItem updateShoppingItem, string userId, CancellationToken cancellationToken);

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="shoppingItemId">The shopping item that is updated.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateQuantityAsync(
            string userId,
            string shoppingItemId,
            int quantityDelta,
            CancellationToken cancellationToken
        );
    }
}
