namespace Warehouse.Api.Services.Domain
{
    using Warehouse.Api.Contracts.Database;
    using Warehouse.Api.Contracts.Services.Atomic;
    using Warehouse.Api.Contracts.Services.Domain;
    using Warehouse.Api.Contracts.Users;

    /// <inheritdoc cref="IDomainUserService" />
    public class DomainUserService : DomainService<ICreateUser, IUser>, IDomainUserService
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DomainUserService" /> class.
        /// </summary>
        /// <param name="transactionHandler">The database transaction handler.</param>
        /// <param name="atomicUserService">The atomic user service.</param>
        public DomainUserService(ITransactionHandler transactionHandler, IAtomicUserService atomicUserService)
            : base(
                transactionHandler,
                atomicUserService)
        {
        }
    }
}
