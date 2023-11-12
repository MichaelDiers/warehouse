namespace Warehouse.Api.Contracts.Users
{
    using Warehouse.Api.Contracts.Database;

    /// <summary>
    ///     A provider for user entries.
    /// </summary>
    public interface IUserProvider : IProvider<IUser>
    {
    }
}
