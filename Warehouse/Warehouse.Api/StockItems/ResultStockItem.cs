namespace Warehouse.Api.StockItems
{
    using Generic.Base.Api.Models;

    /// <summary>
    ///     The stock item data that are sent to the client.
    /// </summary>
    /// <seealso cref="Generic.Base.Api.Models.LinkResult" />
    public class ResultStockItem : LinkResult
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResultStockItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="minimumQuantity">The minimal required quantity.</param>
        /// <param name="links">The links to valid operations.</param>
        public ResultStockItem(
            IEnumerable<Link> links,
            int minimumQuantity,
            string name,
            int quantity
        )
            : base(links)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.MinimumQuantity = minimumQuantity;
        }

        /// <summary>
        ///     Gets the minimum required quantity of the item in stock.
        /// </summary>
        public int MinimumQuantity { get; }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the quantity of the item in stock.
        /// </summary>
        public int Quantity { get; }
    }
}
