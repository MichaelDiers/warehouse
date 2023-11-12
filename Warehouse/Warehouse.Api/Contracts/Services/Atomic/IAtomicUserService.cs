namespace Warehouse.Api.Contracts.Services.Atomic
{
    using Warehouse.Api.Contracts.Users;

    /// <summary>
    ///     An atomic service for user entries.
    /// </summary>
    public interface IAtomicUserService : IAtomicService<ICreateUser, IUser>
    {
    }
}
