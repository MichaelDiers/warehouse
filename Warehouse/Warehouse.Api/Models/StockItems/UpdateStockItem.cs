namespace Warehouse.Api.Models.StockItems
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Warehouse.Api.Contracts.StockItems;

    /// <inheritdoc cref="IUpdateStockItem" />
    public class UpdateStockItem : IUpdateStockItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UpdateStockItem" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="minimumQuantity">The minimum required quantity.</param>
        public UpdateStockItem(
            string id,
            string name,
            int quantity,
            int minimumQuantity
        )
        {
            this.Id = id;
            this.Name = name;
            this.Quantity = quantity;
            this.MinimumQuantity = minimumQuantity;
        }

        /// <summary>
        ///     Gets or sets the id of the item.
        /// </summary>
        [Required]
        public string Id { get; set; }

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
