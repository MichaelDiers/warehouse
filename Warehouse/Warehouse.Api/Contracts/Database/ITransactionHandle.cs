namespace Warehouse.Api.Contracts.Database
{
    using MongoDB.Driver;

    /// <summary>
    ///     A database transaction handle.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ITransactionHandle : IDisposable
    {
        /// <summary>
        ///     Gets the client session handle.
        /// </summary>
        IClientSessionHandle ClientSessionHandle { get; }

        /// <summary>
        ///     Aborts the transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task AbortTransactionAsync(CancellationToken cancellationToken);

        /// <summary>
        ///     Commits the transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
        Task CommitTransactionAsync(CancellationToken cancellationToken);
    }
}
