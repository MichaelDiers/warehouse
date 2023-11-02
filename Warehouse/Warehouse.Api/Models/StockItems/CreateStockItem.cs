namespace Warehouse.Api.Models.StockItems
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Warehouse.Api.Contracts.StockItems;

    /// <inheritdoc cref="ICreateStockItem" />
    public class CreateStockItem : ICreateStockItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateStockItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        public CreateStockItem(string name, int quantity)
        {
            this.Name = name;
            this.Quantity = quantity;
        }

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
            9999)]
        public int Quantity { get; set; }
    }
}
