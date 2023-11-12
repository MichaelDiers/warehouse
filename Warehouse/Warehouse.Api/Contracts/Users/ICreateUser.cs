namespace Warehouse.Api.Contracts.Users
{
    /// <summary>
    ///     The user data for creating a new user.
    /// </summary>
    public interface ICreateUser
    {
        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Gets the password.
        /// </summary>
        string Password { get; }
    }
}
