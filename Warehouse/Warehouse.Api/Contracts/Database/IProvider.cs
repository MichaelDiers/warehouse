namespace Warehouse.Api.Contracts.Database
{
    /// <summary>
    ///     Describes basic operations for providers.
    /// </summary>
    /// <typeparam name="TEntry">The type of the entry.</typeparam>
    public interface IProvider<TEntry>
    {
        /// <summary>
        ///     Creates the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">Handle for database transactions.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task CreateAsync(TEntry entry, CancellationToken cancellationToken, ITransactionHandle transactionHandle);

        /// <summary>
        ///     Delete an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task Delete(string applicationId, CancellationToken cancellationToken, ITransactionHandle transactionHandle);

        /// <summary>
        ///     Delete an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task Delete(string applicationId, CancellationToken cancellationToken);

        /// <summary>
        ///     Read all entries from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        Task<IEnumerable<TEntry>> Read(CancellationToken cancellationToken);

        /// <summary>
        ///     Read all entries from the database.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is a list of all entries.</returns>
        Task<IEnumerable<TEntry>> Read(CancellationToken cancellationToken, ITransactionHandle transactionHandle);

        /// <summary>
        ///     Read an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        Task<TEntry> ReadById(string applicationId, CancellationToken cancellationToken);

        /// <summary>
        ///     Read an entry by its id used in the application.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task{T}" /> whose result is the found entry.</returns>
        Task<TEntry> ReadById(
            string applicationId,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="transactionHandle">The transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task Update(TEntry entry, CancellationToken cancellationToken, ITransactionHandle transactionHandle);

        /// <summary>
        ///     Updates the specified entry.
        /// </summary>
        /// <param name="entry">The entry to be replaced.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task Update(TEntry entry, CancellationToken cancellationToken);
    }
}
