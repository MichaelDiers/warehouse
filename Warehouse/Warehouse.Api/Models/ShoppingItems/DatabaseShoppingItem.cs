namespace Warehouse.Api.Models.ShoppingItems
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using Warehouse.Api.Contracts.ShoppingItems;

    /// <summary>
    ///     Describes a database shopping item.
    /// </summary>
    public class DatabaseShoppingItem
    {
        /// <summary>
        ///     The database collection name for shopping items.
        /// </summary>
        public static readonly string CollectionName = $"{nameof(DatabaseShoppingItem)}s";

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseShoppingItem" /> class.
        /// </summary>
        public DatabaseShoppingItem()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseShoppingItem" /> class.
        /// </summary>
        /// <param name="shoppingItem">The instance is initialized from the data of the given shopping item.</param>
        public DatabaseShoppingItem(IShoppingItem shoppingItem)
        {
            this.Name = shoppingItem.Name;
            this.Quantity = shoppingItem.Quantity;
            this.ShoppingItemId = shoppingItem.Id;
            this.UserId = shoppingItem.UserId;
            this.StockItemId = shoppingItem.StockItemId;
        }

        /// <summary>
        ///     Gets or sets the database id of the item.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     Gets the required quantity of the item.
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the shopping item id of the item.
        /// </summary>
        public string? ShoppingItemId { get; set; }

        /// <summary>
        ///     Gets or sets the stock item identifier.
        /// </summary>
        public string StockItemId { get; set; }

        /// <summary>
        ///     Gets the unique identifier of the owner.
        /// </summary>
        public string? UserId { get; set; }
    }
}
