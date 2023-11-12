namespace Warehouse.Api.Contracts.Users
{
    /// <summary>
    ///     Describes a user.
    /// </summary>
    public interface IUser : IApplicationEntry
    {
        /// <summary>
        ///     Gets the password.
        /// </summary>
        string Password { get; }

        /// <summary>
        ///     Gets the roles.
        /// </summary>
        IEnumerable<Role> Roles { get; }
    }
}
