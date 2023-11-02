namespace Warehouse.Api.Models.StockItems
{
    using System.ComponentModel.DataAnnotations;
    using Warehouse.Api.Contracts.StockItems;

    /// <inheritdoc cref="ICreateStockItem" />
    public class CreateStockItem : ICreateStockItem
    {
        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the quantity of the item in stock.
        /// </summary>
        [Required]
        [Range(
            0,
            9999)]
        public int Quantity { get; set; }
    }
}
