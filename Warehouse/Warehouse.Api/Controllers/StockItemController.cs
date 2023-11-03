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

        /// <summary>
        ///     Deletes the specified stock item identifier.
        /// </summary>
        /// <param name="stockItemId">The stock item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>An <see cref="OkResult" /> if the delete succeeds and <see cref="NotFoundResult" /> otherwise.</returns>
        [HttpDelete("{stockItemId}")]
        public async Task<ActionResult> Delete(
            [BindRequired] [FromRoute] string stockItemId,
            CancellationToken cancellationToken
        )
        {
            var success = await this.stockItemService.DeleteAsync(
                this.User.Claims.RequiredId(),
                stockItemId,
                cancellationToken);
            return success ? this.Ok() : this.NotFound();
        }

        /// <summary>
        ///     Read all stock items of the current user.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A list of stock items.</returns>
        [HttpGet]
        public async Task<IEnumerable<IStockItem>> Get(CancellationToken cancellationToken)
        {
            return await this.stockItemService.ReadAsync(
                this.User.Claims.RequiredId(),
                cancellationToken);
        }

        /// <summary>
        ///     Gets the stock item with the specified id.
        /// </summary>
        /// <param name="stockItemId">The identifier of the stock item.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The stock item with the given id.</returns>
        [HttpGet("{stockItemId}")]
        public async Task<ActionResult<IStockItem>> Get(
            [BindRequired] [FromRoute] string stockItemId,
            CancellationToken cancellationToken
        )
        {
            var result = await this.stockItemService.ReadByIdAsync(
                this.User.Claims.RequiredId(),
                stockItemId,
                cancellationToken);
            if (result is null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        /// <summary>
        ///     Posts the specified stock item.
        /// </summary>
        /// <param name="createStockItem">The stock item to be created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The created stock item.</returns>
        [HttpPost]
        public async Task<ActionResult> Post(
            [FromBody] CreateStockItem createStockItem,
            CancellationToken cancellationToken
        )
        {
            var stockItem = await this.stockItemService.CreateAsync(
                createStockItem,
                this.User.Claims.RequiredId(),
                cancellationToken);
            return this.CreatedAtAction(
                nameof(this.Get),
                new {stockItemId = stockItem.Id},
                stockItem);
        }

        /// <summary>
        ///     Update the specified stock item.
        /// </summary>
        /// <param name="updateStockItem">The stock item to be updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>An <see cref="OkResult" /> if updated succeeds and <see cref="NotFoundResult" /> otherwise.</returns>
        [HttpPut]
        public async Task<ActionResult> Put(
            [FromBody] UpdateStockItem updateStockItem,
            CancellationToken cancellationToken
        )
        {
            var success = await this.stockItemService.UpdateAsync(
                updateStockItem,
                this.User.Claims.RequiredId(),
                cancellationToken);
            return success ? this.Ok() : this.NotFound();
        }
    }
}
