namespace Warehouse.Api.Models.StockItems
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Warehouse.Api.Contracts.StockItems;

    /// <summary>
    ///     Describes a database stock item.
    /// </summary>
    public class DatabaseStockItem
    {
        /// <summary>
        ///     The database collection name for stock items.
        /// </summary>
        public static readonly string CollectionName = $"{nameof(DatabaseStockItem)}s";

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseStockItem" /> class.
        /// </summary>
        public DatabaseStockItem()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseStockItem" /> class.
        /// </summary>
        /// <param name="stockItem">The instance is initialized from the data of the given stock item.</param>
        public DatabaseStockItem(IStockItem stockItem)
        {
            this.Name = stockItem.Name;
            this.Quantity = stockItem.Quantity;
            this.MinimumQuantity = stockItem.MinimumQuantity;
            this.StockItemId = stockItem.Id;
            this.UserId = stockItem.UserId;
        }

        /// <summary>
        ///     Gets or sets the database id of the item.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        ///     Gets or sets the minimum required quantity of the item in stock.
        /// </summary>
        public int? MinimumQuantity { get; set; }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     Gets the quantity of the item in stock.
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the stock item id of the item.
        /// </summary>
        public string? StockItemId { get; set; }

        /// <summary>
        ///     Gets the unique identifier of the owner.
        /// </summary>
        public string? UserId { get; set; }
    }
}
