namespace Warehouse.Api.Contracts.Services.Domain
{
    using Warehouse.Api.Contracts.Users;

    /// <summary>
    ///     The domain service for users.
    /// </summary>
    public interface IDomainUserService : IDomainService<ICreateUser, IUser>
    {
    }
}
