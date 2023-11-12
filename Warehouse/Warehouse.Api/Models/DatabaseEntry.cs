namespace Warehouse.Api.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    ///     A base class for database entries.
    /// </summary>
    public class DatabaseEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseEntry" /> class.
        /// </summary>
        /// <param name="applicationId">The id of the entry used in the application.</param>
        /// <param name="databaseId">The id of the entry used in the database.</param>
        public DatabaseEntry(string applicationId, string? databaseId = null)
        {
            this.ApplicationId = applicationId;
            this.DatabaseId = databaseId;
        }

        /// <summary>
        ///     Gets or sets the application id of the entry.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        ///     Gets or sets the database id of the entry.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DatabaseId { get; set; }
    }
}
