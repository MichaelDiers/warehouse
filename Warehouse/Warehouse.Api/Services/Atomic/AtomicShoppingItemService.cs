namespace Warehouse.Api.Services.Atomic
{
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Models.ShoppingItems;

    /// <inheritdoc cref="IAtomicShoppingItemService" />
    internal class AtomicShoppingItemService : IAtomicShoppingItemService
    {
        /// <summary>
        ///     The provider for shopping items.
        /// </summary>
        private readonly IShoppingItemProvider provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicShoppingItemService" /> class.
        /// </summary>
        /// <param name="provider">The provider for shopping items.</param>
        public AtomicShoppingItemService(IShoppingItemProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        ///     Creates the specified shopping item.
        /// </summary>
        /// <param name="createShoppingItem">The shopping item to be created.</param>
        /// <param name="userId">The unique id of the user.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is the created shopping item.</returns>
        public async Task<IShoppingItem> CreateAsync(
            ICreateShoppingItem createShoppingItem,
            string userId,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var shoppingItem = new ShoppingItem(
                Guid.NewGuid().ToString(),
                createShoppingItem.Name,
                createShoppingItem.Quantity,
                userId,
                createShoppingItem.StockItemId);

            await this.provider.CreateAsync(
                shoppingItem,
                cancellationToken);

            return shoppingItem;
        }

        /// <summary>
        ///     Deletes the specified shopping item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{TResult}" /> whose result is true if the item is deleted and false otherwise.</returns>
        public Task<bool> DeleteAsync(string userId, string shoppingItemId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return this.provider.DeleteAsync(
                userId,
                shoppingItemId,
                cancellationToken);
        }

        /// <summary>
        ///     Reads all shopping items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>All shopping items with the specified user id.</returns>
        public Task<IEnumerable<IShoppingItem>> ReadAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return this.provider.ReadAsync(
                userId,
                cancellationToken);
        }

        /// <summary>
        ///     Reads a shopping item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The found shopping item.</returns>
        public Task<IShoppingItem?> ReadByIdAsync(
            string userId,
            string shoppingItemId,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            return this.provider.ReadByIdAsync(
                userId,
                shoppingItemId,
                cancellationToken);
        }

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="shoppingItemId">The shopping item that is updated.</param>
        /// <param name="operation">Specifies the type of the update.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        public async Task<bool> UpdateAsync(
            string userId,
            string shoppingItemId,
            UpdateOperation operation,
            int quantityDelta,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (quantityDelta == 0)
            {
                return true;
            }

            // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
            var delta = operation switch
            {
                UpdateOperation.Increase => quantityDelta,
                UpdateOperation.Decrease => -quantityDelta,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(operation),
                    operation,
                    null)
            };

            return await this.provider.UpdateAsync(
                userId,
                shoppingItemId,
                delta,
                cancellationToken);
        }

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="updateShoppingItem">The shopping item that is updated.</param>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is true if the update is executed and false otherwise.</returns>
        public Task<bool> UpdateAsync(
            IUpdateShoppingItem updateShoppingItem,
            string userId,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var shoppingItem = new ShoppingItem(
                updateShoppingItem.Id,
                updateShoppingItem.Name,
                updateShoppingItem.Quantity,
                userId,
                updateShoppingItem.StockItemId);

            return this.provider.UpdateAsync(
                shoppingItem,
                cancellationToken);
        }
    }
}
