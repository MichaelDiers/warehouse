namespace Warehouse.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.StockItems;
    using Warehouse.Api.Validation;

    /// <summary>
    ///     The controller for manipulating stock items.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    [GuidValidation("stockItemId")]
    public class StockItemController : ControllerBase
    {
        /// <summary>
        ///     The business logic for handling stock items.
        /// </summary>
        private readonly IStockItemService stockItemService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItemController" /> class.
        /// </summary>
        /// <param name="stockItemService">The business logic for handling stock items.</param>
        public StockItemController(IStockItemService stockItemService)
        {
            this.stockItemService = stockItemService;
        }

        // DELETE api/<StockItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        ///     Read all stock items of the current user.
        /// </summary>
        /// <returns>A list of stock items.</returns>
        [HttpGet]
        public async Task<IEnumerable<IStockItem>> Get()
        {
            return await this.stockItemService.ReadAsync(this.User.Claims.RequiredId());
        }

        /// <summary>
        ///     Gets the stock item with the specified id.
        /// </summary>
        /// <param name="stockItemId">The identifier of the stock item.</param>
        /// <returns>The stock item with the given id.</returns>
        [HttpGet("{stockItemId}")]
        public async Task<IStockItem> Get([BindRequired] [FromRoute] string stockItemId)
        {
            return await this.stockItemService.ReadByIdAsync(
                this.User.Claims.RequiredId(),
                stockItemId);
        }

        /// <summary>
        ///     Posts the specified stock item.
        /// </summary>
        /// <param name="createStockItem">The stock item to be created.</param>
        /// <returns>The created stock item.</returns>
        [HttpPost]
        public async Task<IStockItem> Post([FromBody] CreateStockItem createStockItem)
        {
            return await this.stockItemService.CreateAsync(
                createStockItem,
                this.User.Claims.RequiredId());
        }

        // PUT api/<StockItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
