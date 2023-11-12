namespace Warehouse.Api.Services.Atomic
{
    using Warehouse.Api.Contracts.Services.Atomic;
    using Warehouse.Api.Contracts.Users;
    using Warehouse.Api.Models.Users;

    /// <inheritdoc cref="IAtomicUserService" />
    public class AtomicUserService : AtomicService<ICreateUser, IUser>, IAtomicUserService
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AtomicService{TCreate, TEntry}" /> class.
        /// </summary>
        /// <param name="userProvider">The database provider for users.</param>
        public AtomicUserService(IUserProvider userProvider)
            : base(
                userProvider,
                AtomicUserService.ToEntry)
        {
        }

        /// <summary>
        ///     Converts an <see cref="ICreateUser" /> to an <see cref="IUser" />.
        /// </summary>
        /// <param name="user">The user to transformed.</param>
        /// <returns>An <see cref="IUser" />.</returns>
        public static IUser ToEntry(ICreateUser user)
        {
            return new User(
                user.Id,
                user.Password,
                new[] {Role.User});
        }
    }
}
