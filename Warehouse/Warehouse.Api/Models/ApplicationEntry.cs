namespace Warehouse.Api.Models
{
    /// <summary>
    ///     Describes an application entry.
    /// </summary>
    public class ApplicationEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ApplicationEntry" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public ApplicationEntry(string id)
        {
            this.Id = id;
        }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }
    }
}
