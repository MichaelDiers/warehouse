namespace Warehouse.Api.Models.StockItems
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Warehouse.Api.Contracts.StockItems;

    /// <inheritdoc cref="ICreateStockItem" />
    public class CreateStockItem : ICreateStockItem
    {
        /// <summary>
        ///     The maximum allowed quantity.
        /// </summary>
        public const int MaxQuantity = 9999;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateStockItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="minimumQuantity">The minimal required quantity.</param>
        public CreateStockItem(string name, int quantity, int minimumQuantity)
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
        public int MinimumQuantity { get; set; }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the quantity of the item in stock.
        /// </summary>
        [BindRequired]
        [Range(
            0,
            CreateStockItem.MaxQuantity)]
        public int Quantity { get; set; }
    }
}
