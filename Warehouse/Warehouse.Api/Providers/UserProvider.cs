namespace Warehouse.Api.Providers
{
    using MongoDB.Driver;
    using Warehouse.Api.Contracts.Config;
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.Users;
    using Warehouse.Api.Models.Users;

    /// <summary>
    ///     A database provider for user entries.
    /// </summary>
    public class UserProvider : Provider<IUser, DatabaseUser>, IUserProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Provider{TEntry, TDatabase}" /> class.
        /// </summary>
        /// <param name="mongoClient">The mongo client.</param>
        /// <param name="appConfiguration">Access the application configuration.</param>
        /// <param name="transactionHandler">A handler for database transactions.</param>
        public UserProvider(
            IMongoClient mongoClient,
            IAppConfiguration appConfiguration,
            ITransactionHandler transactionHandler
        )
            : base(
                mongoClient,
                appConfiguration.Warehouse.DatabaseName,
                DatabaseUser.CollectionName,
                user => new DatabaseUser(user),
                UserProvider.FromDatabaseUser,
                transactionHandler)
        {
        }

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public override async Task Update(
            IUser entry,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.Update(
                doc => string.Equals(
                    doc.ApplicationId,
                    entry.Id,
                    StringComparison.OrdinalIgnoreCase),
                Builders<DatabaseUser>.Update.Set(
                        doc => doc.Password,
                        entry.Password)
                    .Set(
                        doc => doc.Roles,
                        entry.Roles),
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Transform a <see cref="DatabaseUser" /> to an <see cref="IUser" />.
        /// </summary>
        /// <param name="databaseUser">The database user.</param>
        /// <returns>The transformed user.</returns>
        private static IUser FromDatabaseUser(DatabaseUser databaseUser)
        {
            return new User(
                databaseUser.ApplicationId,
                databaseUser.Password,
                databaseUser.Roles.ToArray());
        }
    }
}
