namespace Warehouse.Api.Models.Users
{
    using Warehouse.Api.Contracts.Users;

    /// <inheritdoc cref="IUser" />
    public class User : ApplicationEntry, IUser
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="password">The password.</param>
        /// <param name="roles">The roles.</param>
        public User(string id, string password, IEnumerable<Role> roles)
            : base(id)
        {
            this.Password = password;
            this.Roles = roles.ToArray();
        }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the roles.
        /// </summary>
        public IEnumerable<Role> Roles { get; set; }
    }
}
