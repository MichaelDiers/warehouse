namespace Warehouse.Api.Contracts.Database
{
    /// <summary>
    ///     A handler for database transactions.
    /// </summary>
    public interface ITransactionHandler
    {
        /// <summary>
        ///     Starts a new transaction.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A <see cref="Task" /> whose result is a <see cref="ITransactionHandle" />.</returns>
        Task<ITransactionHandle> StartTransactionAsync(CancellationToken cancellationToken);
    }
}
