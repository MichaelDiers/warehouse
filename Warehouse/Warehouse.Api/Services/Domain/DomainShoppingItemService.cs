namespace Warehouse.Api.Services.Domain
{
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.ShoppingItems;

    /// <inheritdoc cref="IDomainShoppingItemService" />
    public class DomainShoppingItemService : IDomainShoppingItemService
    {
        /// <summary>
        ///     The atomic shopping item service.
        /// </summary>
        private readonly IAtomicShoppingItemService atomicShoppingItemService;

        /// <summary>
        ///     A database transaction handler.
        /// </summary>
        private readonly ITransactionHandler transactionHandler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DomainShoppingItemService" /> class.
        /// </summary>
        /// <param name="atomicShoppingItemService">The atomic shopping item service.</param>
        /// <param name="transactionHandler">A database transaction handler.</param>
        public DomainShoppingItemService(
            IAtomicShoppingItemService atomicShoppingItemService,
            ITransactionHandler transactionHandler
        )
        {
            this.atomicShoppingItemService = atomicShoppingItemService;
            this.transactionHandler = transactionHandler;
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
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = await this.atomicShoppingItemService.CreateAsync(
                    createShoppingItem,
                    userId,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Deletes the specified shopping item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(string userId, string shoppingItemId, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.atomicShoppingItemService.DeleteAsync(
                    userId,
                    shoppingItemId,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
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

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="updateShoppingItem">The shopping item that is updated.</param>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(
            IUpdateShoppingItem updateShoppingItem,
            string userId,
            CancellationToken cancellationToken
        )
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.atomicShoppingItemService.UpdateAsync(
                    updateShoppingItem,
                    userId,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="shoppingItemId">The shopping item that is updated.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateQuantityAsync(
            string userId,
            string shoppingItemId,
            int quantityDelta,
            CancellationToken cancellationToken
        )
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.atomicShoppingItemService.UpdateQuantityAsync(
                    userId,
                    shoppingItemId,
                    quantityDelta,
                    cancellationToken,
                    session);
                await session.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await session.AbortTransactionAsync(cancellationToken);
            }
        }
    }
}
