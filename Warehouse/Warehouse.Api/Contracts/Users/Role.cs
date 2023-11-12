namespace Warehouse.Api.Contracts.Users
{
    /// <summary>
    ///     Describes the user roles.
    /// </summary>
    public enum Role
    {
        /// <summary>
        ///     User has no rule.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The user role.
        /// </summary>
        User,

        /// <summary>
        ///     The admin role.
        /// </summary>
        Admin
    }
}
