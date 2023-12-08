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
        public UpdateStockItem(int minimumQuantity, string name, int quantity)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.MinimumQuantity = minimumQuantity;
        }

        /// <summary>
        ///     Gets the minimum required quantity of the item in stock.
        /// </summary>
        [BindRequired]
        [Range(
            CreateStockItem.MinQuantity,
            CreateStockItem.MaxQuantity)]
        public int MinimumQuantity { get; }

        /// <summary>
        ///     Gets the name of the item.
        /// </summary>
        [Required]
        [StringLength(
            CreateStockItem.NameMaxLength,
            MinimumLength = CreateStockItem.NameMinLength)]
        public string Name { get; }

        /// <summary>
        ///     Gets the quantity of the item in stock.
        /// </summary>
        [BindRequired]
        [Range(
            CreateStockItem.MinQuantity,
            CreateStockItem.MaxQuantity)]
        public int Quantity { get; }
    }
}
