namespace Warehouse.Api.StockItems
{
    using Generic.Base.Api.Models;

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
            string name,
            int quantity,
            int minimumQuantity,
            IEnumerable<Link> links
        )
            : base(links)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.MinimumQuantity = minimumQuantity;
        }

        /// <summary>
        ///     Gets or sets the minimum required quantity of the item in stock.
        /// </summary>
        public int MinimumQuantity { get; set; }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the quantity of the item in stock.
        /// </summary>
        public int Quantity { get; set; }
    }
}
