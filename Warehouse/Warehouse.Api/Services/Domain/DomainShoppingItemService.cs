namespace Warehouse.Api.Services.Domain
{
    using Warehouse.Api.Contracts.ShoppingItems;

    /// <inheritdoc cref="IDomainShoppingItemService" />
    public class DomainShoppingItemService : IDomainShoppingItemService
    {
        /// <summary>
        ///     The atomic shopping item service.
        /// </summary>
        private readonly IAtomicShoppingItemService atomicShoppingItemService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DomainShoppingItemService" /> class.
        /// </summary>
        /// <param name="atomicShoppingItemService">The atomic shopping item service.</param>
        public DomainShoppingItemService(IAtomicShoppingItemService atomicShoppingItemService)
        {
            this.atomicShoppingItemService = atomicShoppingItemService;
        }

        /// <summary>
        ///     Reads all shopping items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>All shopping items with the specified user id.</returns>
        public Task<IEnumerable<IShoppingItem>> ReadAsync(string userId, CancellationToken cancellationToken)
        {
            return this.atomicShoppingItemService.ReadAsync(
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
        public Task<IShoppingItem> ReadByIdAsync(
            string userId,
            string shoppingItemId,
            CancellationToken cancellationToken
        )
        {
            return this.atomicShoppingItemService.ReadByIdAsync(
                userId,
                shoppingItemId,
                cancellationToken);
        }
    }
}
