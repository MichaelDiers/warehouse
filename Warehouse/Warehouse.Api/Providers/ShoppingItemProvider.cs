﻿namespace Warehouse.Api.Providers
{
    using MongoDB.Driver;
    using Warehouse.Api.Contracts.Config;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Models.ShoppingItems;

    /// <inheritdoc cref="IShoppingItemProvider" />
    internal class ShoppingItemProvider : BaseProvider, IShoppingItemProvider
    {
        /// <summary>
        ///     The collection that contains shopping items.
        /// </summary>
        private readonly IMongoCollection<DatabaseShoppingItem> shoppingItemCollection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShoppingItemProvider" /> class.
        /// </summary>
        /// <param name="mongoClient">The mongo client.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="transactionHandler">A handler for database transactions.</param>
        public ShoppingItemProvider(
            IMongoClient mongoClient,
            IAppConfiguration configuration,
            ITransactionHandler transactionHandler
        )
            : base(
                nameof(StockItemProvider),
                transactionHandler)
        {
            this.shoppingItemCollection = mongoClient.GetDatabase(configuration.Warehouse.DatabaseName)
                .GetCollection<DatabaseShoppingItem>(DatabaseShoppingItem.CollectionName);
        }

        /// <summary>
        ///     Creates the specified shopping item.
        /// </summary>
        /// <param name="shoppingItem">The shopping item to be created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task CreateAsync(
            IShoppingItem shoppingItem,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.shoppingItemCollection.InsertOneAsync(
                    transactionHandle.ClientSessionHandle,
                    new DatabaseShoppingItem(shoppingItem),
                    cancellationToken: cancellationToken));
        }

        /// <summary>
        ///     Deletes the specified shopping item.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(
            string userId,
            string shoppingItemId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.shoppingItemCollection.DeleteOneAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.UserId == userId && doc.ShoppingItemId == shoppingItemId,
                    cancellationToken: cancellationToken));
        }

        /// <summary>
        ///     Deletes the specified shopping item by its stock item id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteByStockItemIdAsync(
            string userId,
            string stockItemId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.shoppingItemCollection.DeleteOneAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.UserId == userId && doc.StockItemId == stockItemId,
                    cancellationToken: cancellationToken));
        }

        /// <summary>
        ///     Reads all shopping items of the given user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>All shopping items with the specified user id.</returns>
        public async Task<IEnumerable<IShoppingItem>> ReadAsync(string userId, CancellationToken cancellationToken)
        {
            var result = await this.Execute(
                () => this.shoppingItemCollection.FindAsync(
                    doc => doc.UserId == userId,
                    cancellationToken: cancellationToken));

            return await ShoppingItemProvider.ToShoppingItems(
                result,
                cancellationToken);
        }

        /// <summary>
        ///     Reads a shopping item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>The found shopping item.</returns>
        public async Task<IShoppingItem> ReadByIdAsync(
            string userId,
            string shoppingItemId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            return await this.Execute(
                () => this.shoppingItemCollection.FindAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.UserId == userId && doc.ShoppingItemId == shoppingItemId,
                    cancellationToken: cancellationToken),
                ShoppingItemProvider.ToShoppingItem,
                cancellationToken);
        }

        /// <summary>
        ///     Reads a shopping item by its identifier.
        /// </summary>
        /// <param name="userId">The user identifier of the owner.</param>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The found shopping item.</returns>
        public async Task<IShoppingItem> ReadByIdAsync(
            string userId,
            string shoppingItemId,
            CancellationToken cancellationToken
        )
        {
            return await this.Execute(
                session => this.ReadByIdAsync(
                    userId,
                    shoppingItemId,
                    cancellationToken,
                    session),
                cancellationToken);
        }

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="shoppingItem">The shopping item that is updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(
            IShoppingItem shoppingItem,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.shoppingItemCollection.UpdateOneAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.ShoppingItemId == shoppingItem.Id &&
                           doc.UserId == shoppingItem.UserId &&
                           doc.StockItemId == shoppingItem.StockItemId,
                    Builders<DatabaseShoppingItem>.Update.Set(
                            doc => doc.Name,
                            shoppingItem.Name)
                        .Set(
                            doc => doc.Quantity,
                            shoppingItem.Quantity),
                    cancellationToken: cancellationToken));
        }

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="shoppingItemId">The shopping item that is updated.</param>
        /// <param name="quantityDelta">The quantity is updated by this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateQuantityAsync(
            string userId,
            string shoppingItemId,
            int quantityDelta,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.shoppingItemCollection.UpdateOneAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.ShoppingItemId == shoppingItemId && doc.UserId == userId,
                    Builders<DatabaseShoppingItem>.Update.Inc(
                        doc => doc.Quantity,
                        quantityDelta),
                    cancellationToken: cancellationToken));
        }

        /// <summary>
        ///     Updates the specified shopping item.
        /// </summary>
        /// <param name="userId">The id of the owner.</param>
        /// <param name="stockItemId">The referenced stock item id of the shopping item.</param>
        /// <param name="quantity">The quantity is updated to this amount.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateQuantityByStockItemIdAsync(
            string userId,
            string stockItemId,
            int quantity,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Execute(
                () => this.shoppingItemCollection.UpdateOneAsync(
                    transactionHandle.ClientSessionHandle,
                    doc => doc.StockItemId == stockItemId && doc.UserId == userId,
                    Builders<DatabaseShoppingItem>.Update.Set(
                        doc => doc.Quantity,
                        quantity),
                    cancellationToken: cancellationToken));
        }

        /// <summary>
        ///     Converts from <see cref="DatabaseShoppingItem" /> to <see cref="IShoppingItem" />.
        /// </summary>
        /// <param name="databaseShoppingItem">The database shopping item.</param>
        /// <returns>The converted shopping item.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private static IShoppingItem ToShoppingItem(DatabaseShoppingItem databaseShoppingItem)
        {
            if (databaseShoppingItem.Name is null ||
                databaseShoppingItem.Quantity is null ||
                databaseShoppingItem.ShoppingItemId is null ||
                databaseShoppingItem.UserId is null)
            {
                // Todo
                throw new NotImplementedException();
            }

            return new ShoppingItem(
                databaseShoppingItem.ShoppingItemId,
                databaseShoppingItem.Name,
                databaseShoppingItem.Quantity.Value,
                databaseShoppingItem.UserId,
                databaseShoppingItem.StockItemId);
        }

        /// <summary>
        ///     Converts from <see cref="IEnumerable{T}" /> of <see cref="DatabaseShoppingItem" /> to <see cref="IEnumerable{T}" />
        ///     of
        ///     <see cref="IShoppingItem" />.
        /// </summary>
        /// <param name="cursor">The database cursor.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The converted shopping items.</returns>
        private static async Task<IEnumerable<IShoppingItem>> ToShoppingItems(
            IAsyncCursor<DatabaseShoppingItem> cursor,
            CancellationToken cancellationToken
        )
        {
            var result = new List<IShoppingItem>();
            await cursor.ForEachAsync(
                databaseShoppingItem =>
                {
                    result.Add(
                        ShoppingItemProvider.ToShoppingItem(databaseShoppingItem) ??
                        throw new NotImplementedException());
                },
                cancellationToken);
            return result;
        }
    }
}
