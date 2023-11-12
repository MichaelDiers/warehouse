namespace Warehouse.Api.Services.Atomic
{
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.Services.Atomic;

    /// <inheritdoc cref="IAtomicService{TCreate,TEntry}" />
    public class AtomicService<TCreate, TEntry> : IAtomicService<TCreate, TEntry>
    {
        /// <summary>
        ///     The database provider.
        /// </summary>
        private readonly IProvider<TEntry> provider;

        /// <summary>
        ///     Transform an entry from <typeparamref name="TCreate" /> to <typeparamref name="TEntry" />.
        /// </summary>
        private readonly Func<TCreate, TEntry> toEntry;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicService{TCreate, TEntry}" /> class.
        /// </summary>
        /// <param name="provider">The database provider.</param>
        /// <param name="toEntry">Transform an entry from <typeparamref name="TCreate" /> to <typeparamref name="TEntry" />.</param>
        protected AtomicService(IProvider<TEntry> provider, Func<TCreate, TEntry> toEntry)
        {
            this.provider = provider;
            this.toEntry = toEntry;
        }

        /// <summary>
        ///     Creates a new entry.
        /// </summary>
        /// <param name="createEntry">The create entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns></returns>
        public async Task<TEntry> CreateAsync(
            TCreate createEntry,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            var entry = this.toEntry(createEntry);
            await this.provider.CreateAsync(
                entry,
                cancellationToken,
                transactionHandle);
            return entry;
        }

        /// <summary>
        ///     Deletes the entry by its id.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns></returns>
        public async Task DeleteAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.provider.Delete(
                id,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Reads all entries.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntry>> ReadAsync(
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            return await this.provider.Read(
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Read entry by its id used in the application.
        /// </summary>
        /// <param name="id">The id of the entry.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns></returns>
        public async Task<TEntry> ReadByIdAsync(
            string id,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            return await this.provider.ReadById(
                id,
                cancellationToken,
                transactionHandle);
        }

        /// <summary>
        ///     Updates the entry by its id.
        /// </summary>
        /// <param name="entry">The entry to be updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <param name="transactionHandle">The database transaction handle.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        public async Task UpdateAsync(
            TEntry entry,
            CancellationToken cancellationToken,
            ITransactionHandle transactionHandle
        )
        {
            await this.provider.Update(
                entry,
                cancellationToken,
                transactionHandle);
        }
    }
}
