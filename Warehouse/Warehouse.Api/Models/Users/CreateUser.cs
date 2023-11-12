namespace Warehouse.Api.Models.Users
{
    using Warehouse.Api.Contracts.Users;

    /// <inheritdoc cref="ICreateUser" />
    public class CreateUser : ICreateUser
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateUser" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="password">The password.</param>
        public CreateUser(string id, string password)
        {
            this.Id = id;
            this.Password = password;
        }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}
