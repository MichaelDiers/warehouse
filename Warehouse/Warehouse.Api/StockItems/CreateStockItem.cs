namespace Warehouse.Api.StockItems
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    /// <summary>
    ///     The required data for creating a stock item.
    /// </summary>
    public class CreateStockItem
    {
        /// <summary>
        ///     The maximum allowed quantity.
        /// </summary>
        public const int MaxQuantity = 9999;

        /// <summary>
        ///     The minimum allowed quantity.
        /// </summary>
        public const int MinQuantity = 0;

        /// <summary>
        ///     The maximum length of the name.
        /// </summary>
        public const int NameMaxLength = 100;

        /// <summary>
        ///     The minimum length of the name.
        /// </summary>
        public const int NameMinLength = 2;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateStockItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="minimumQuantity">The minimal required quantity.</param>
        public CreateStockItem(int minimumQuantity, string name, int quantity)
        {
            this.MinimumQuantity = minimumQuantity;
            this.Name = name;
            this.Quantity = quantity;
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
