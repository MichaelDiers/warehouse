namespace Warehouse.Api.Models.Users
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Warehouse.Api.Contracts.Users;

    /// <summary>
    ///     The database representation of a user.
    /// </summary>
    public class DatabaseUser : DatabaseEntry
    {
        /// <summary>
        ///     The database collection name.
        /// </summary>
        public static readonly string CollectionName = $"{nameof(User)}s";

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseUser" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public DatabaseUser(IUser user)
            : base(user.Id)
        {
            this.Roles = user.Roles.ToArray();
            this.Password = user.Password;
        }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the roles.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        public IEnumerable<Role> Roles { get; set; }
    }
}
