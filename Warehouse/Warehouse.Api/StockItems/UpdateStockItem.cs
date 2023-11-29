namespace Warehouse.Api.StockItems
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    /// <summary>
    ///     The data for updating a stock item.
    /// </summary>
    public class UpdateStockItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateStockItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="minimumQuantity">The minimum required quantity.</param>
        public UpdateStockItem(string name, int quantity, int minimumQuantity)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.MinimumQuantity = minimumQuantity;
        }

        /// <summary>
        ///     Gets or sets the minimum required quantity of the item in stock.
        /// </summary>
        [BindRequired]
        [Range(
            0,
            CreateStockItem.MaxQuantity)]
        public int MinimumQuantity { get; }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; }

        /// <summary>
        ///     Gets or sets the quantity of the item in stock.
        /// </summary>
        [BindRequired]
        [Range(
            0,
            CreateStockItem.MaxQuantity)]
        public int Quantity { get; }
    }
}
