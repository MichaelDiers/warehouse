namespace Warehouse.Api.Providers
{
    using System.Linq.Expressions;
    using MongoDB.Driver;
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Exceptions;
    using Warehouse.Api.Models;
    using Warehouse.Api.Properties;

    /// <summary>
    ///     Describes basic operations for providers.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    /// <typeparam name="TDatabase">The type of the database entry.</typeparam>
    /// <seealso cref="Warehouse.Api.Contracts.Database.IProvider&lt;TEntry&gt;" />
    public abstract class Provider<TEntry, TDatabase> : IProvider<TEntry>
        where TDatabase : DatabaseEntry where TEntry : IApplicationEntry
    {
        /// <summary>
        ///     The database collection for <typeparamref name="TDatabase" /> entries.
        /// </summary>
        private readonly IMongoCollection<TDatabase> collection;

        /// <summary>
        ///     A function to transform a <typeparamref name="TEntry" /> to <typeparamref name="TDatabase" />.
        /// </summary>
        private readonly Func<TEntry, TDatabase> toDatabase;

        /// <summary>
        ///     A function to transform a <typeparamref name="TDatabase" /> to <typeparamref name="TEntry" />.
        /// </summary>
        private readonly Func<TDatabase, TEntry> toEntry;

        /// <summary>
        ///     A handler for database transactions.
        /// </summary>
        private readonly ITransactionHandler transactionHandler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Provider{TEntry, TDatabase}" /> class.
        /// </summary>
        /// <param name="mongoClient">The mongo client.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="toDatabase">A function to transform a <typeparamref name="TEntry" /> to <typeparamref name="TDatabase" />.</param>
        /// <param name="toEntry">A function to transform a <typeparamref name="TDatabase" /> to <typeparamref name="TEntry" />.</param>
        /// <param name="transactionHandler">A handler for database transactions.</param>
        protected Provider(
            IMongoClient mongoClient,
            string databaseName,
            string collectionName,
            Func<TEntry, TDatabase> toDatabase,
            Func<TDatabase, TEntry> toEntry,
            ITransactionHandler transactionHandler
        )
        {
            this.toDatabase = toDatabase;
            this.toEntry = toEntry;
            this.transactionHandler = transactionHandler;
            this.collection = mongoClient.GetDatabase(databaseName).GetCollection<TDatabase>(collectionName);
        }

        /// <summary>
        ///     Creates the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">Handle for database transactions.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task CreateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            var databaseEntry = this.toDatabase(entry);

            try
            {
                await this.collection.InsertOneAsync(
                    transactionHandle.ClientSessionHandle,
                    databaseEntry,
                    cancellationToken: cancellationToken);
            }
            catch (MongoWriteException e) when (e.WriteError.Code == 11000)
            {
                throw new ConflictException(
                    this.GetExceptionMessage(nameof(ConflictException)),
                    e);
            }
            catch (MongoWriteException e) when (e.WriteError.Code == 121)
            {
                throw new BadRequestException(
                    this.GetExceptionMessage(nameof(BadRequestException)),
                    e);
            }
        }

        /// <summary>
        ///     DeleteAsync an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(
            string applicationId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            var result = await this.collection.DeleteOneAsync(
                transactionHandle.ClientSessionHandle,
                doc => string.Equals(
                    doc.ApplicationId,
                    applicationId,
                    StringComparison.OrdinalIgnoreCase),
                cancellationToken: cancellationToken);
            if (!result.IsAcknowledged || result.DeletedCount == 0)
            {
                throw new NotFoundException(this.GetExceptionMessage(nameof(NotFoundException)));
            }
        }

        /// <summary>
        ///     DeleteAsync an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task DeleteAsync(string applicationId, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.DeleteAsync(
                    applicationId,
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
        ///     Read all entries from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        public async Task<IEnumerable<TEntry>> ReadAsync(CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = await this.ReadAsync(
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
        ///     Read all entries from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        public async Task<IEnumerable<TEntry>> ReadAsync(
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            var cursor = await this.collection.FindAsync(
                transactionHandle.ClientSessionHandle,
                doc => true,
                cancellationToken: cancellationToken);
            var result = await cursor.ToListAsync(cancellationToken);
            return result.Select(this.toEntry).ToArray();
        }

        /// <summary>
        ///     Read an entry by its id used in the application.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        public async Task<TEntry> ReadByIdAsync(string applicationId, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                var result = await this.ReadByIdAsync(
                    applicationId,
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
        ///     Read an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        public async Task<TEntry> ReadByIdAsync(
            string applicationId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            var result = await this.collection.FindAsync(
                transactionHandle.ClientSessionHandle,
                doc => string.Equals(
                    doc.ApplicationId,
                    applicationId,
                    StringComparison.OrdinalIgnoreCase),
                cancellationToken: cancellationToken);
            var entry = result.FirstOrDefault(cancellationToken);
            if (entry is null)
            {
                throw new NotFoundException(this.GetExceptionMessage(nameof(NotFoundException)));
            }

            return this.toEntry(entry);
        }

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public abstract Task UpdateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(TEntry entry, CancellationToken cancellationToken)
        {
            using var session = await this.transactionHandler.StartTransactionAsync(cancellationToken);
            try
            {
                await this.UpdateAsync(
                    entry,
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
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="filter">The filter definition.</param>
        /// <param name="update">The update definition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        protected async Task UpdateAsync(
            Expression<Func<TDatabase, bool>> filter,
            UpdateDefinition<TDatabase> update,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            try
            {
                var result = await this.collection.UpdateOneAsync(
                    transactionHandle.ClientSessionHandle,
                    filter,
                    update,
                    cancellationToken: cancellationToken);
                if (!result.IsAcknowledged || result.MatchedCount == 0)
                {
                    throw new NotFoundException(this.GetExceptionMessage(nameof(NotFoundException)));
                }
            }
            catch (MongoWriteException e) when (e.WriteError.Code == 121)
            {
                throw new BadRequestException(
                    this.GetExceptionMessage(nameof(BadRequestException)),
                    e);
            }
        }

        /// <summary>
        ///     Gets the exception message from the resources.
        /// </summary>
        /// <param name="exception">The type name of the exception.</param>
        /// <returns>The exception message or an empty string.</returns>
        private string GetExceptionMessage(string exception)
        {
            var key = $"{this.GetType().Name}_{exception}";
            return Resources.ResourceManager.GetString(key) ?? string.Empty;
        }
    }
}
