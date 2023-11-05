namespace Warehouse.Api.Models.ShoppingItems
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Warehouse.Api.Contracts.ShoppingItems;

    /// <inheritdoc cref="ICreateShoppingItem" />
    public class CreateShoppingItem : ICreateShoppingItem
    {
        /// <summary>
        ///     The maximum allowed quantity.
        /// </summary>
        public const int MaxQuantity = 9999;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateShoppingItem" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="stockItemId">The stock item identifier.</param>
        public CreateShoppingItem(string name, int quantity, string stockItemId)
        {
            this.Name = name;
            this.Quantity = quantity;
            this.StockItemId = stockItemId;
        }

        /// <summary>
        ///     Gets or sets the name of the item.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the required quantity of the item.
        /// </summary>
        [BindRequired]
        [Range(
            0,
            CreateShoppingItem.MaxQuantity)]
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the stock item identifier.
        /// </summary>
        public string StockItemId { get; set; }
    }
}
