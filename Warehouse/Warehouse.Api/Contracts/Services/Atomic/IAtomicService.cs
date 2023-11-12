namespace Warehouse.Api.Contracts.Services.Atomic
{
    using Warehouse.Api.Contracts.Database;

    /// <summary>
    ///     Base description of an atomic service.
    /// </summary>
    public interface IAtomicService<TCreate, TEntry>
    {
        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="createEntry">The create entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns></returns>
        Task<TEntry> CreateAsync(
            TCreate createEntry,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );

        /// <summary>
        ///     Deletes the entry by its id.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns></returns>
        Task DeleteAsync(string id, CancellationToken cancellationToken, ITransactionHandle transactionHandle);

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntry>> ReadAsync(CancellationToken cancellationToken, ITransactionHandle transactionHandle);

        /// <summary>
        ///     Read entry by its id used in the application.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns></returns>
        Task<TEntry> ReadByIdAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        );

        /// <summary>
        ///     Updates the entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task UpdateAsync(TEntry entry, CancellationToken cancellationToken, ITransactionHandle transactionHandle);
    }
}
