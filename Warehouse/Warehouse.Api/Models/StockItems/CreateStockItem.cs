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
            CreateStockItem.MinQuantity,
            CreateStockItem.MaxQuantity)]
        public int MinimumQuantity { get; set; }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        [Required]
        [StringLength(
            CreateStockItem.NameMaxLength,
            MinimumLength = CreateStockItem.NameMinLength)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the quantity of the item in stock.
        /// </summary>
        [BindRequired]
        [Range(
            CreateStockItem.MinQuantity,
            CreateStockItem.MaxQuantity)]
        public int Quantity { get; set; }
    }
}
