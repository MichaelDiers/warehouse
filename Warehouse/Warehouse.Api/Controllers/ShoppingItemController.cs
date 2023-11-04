namespace Warehouse.Api.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.ShoppingItems;
    using Warehouse.Api.Validation;

    /// <summary>
    ///     The controller for manipulating shopping items.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    [GuidValidation("shoppingItemId")]
    public class ShoppingItemController : ControllerBase
    {
        /// <summary>
        ///     The decrease operation for <see cref="Put(string, string, int, System.Threading.CancellationToken)" />.
        /// </summary>
        private const string OperationDecrease = "decrease";

        /// <summary>
        ///     The increase operation for <see cref="Put(string, string, int, System.Threading.CancellationToken)" />.
        /// </summary>
        private const string OperationIncrease = "increase";

        /// <summary>
        ///     The business logic for handling shopping items.
        /// </summary>
        private readonly IShoppingItemService shoppingItemService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShoppingItemController" /> class.
        /// </summary>
        /// <param name="shoppingItemService">The business logic for handling shopping items.</param>
        public ShoppingItemController(IShoppingItemService shoppingItemService)
        {
            this.shoppingItemService = shoppingItemService;
        }

        /// <summary>
        ///     Deletes the specified shopping item identifier.
        /// </summary>
        /// <param name="shoppingItemId">The shopping item identifier.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>An <see cref="OkResult" /> if the delete succeeds and <see cref="NotFoundResult" /> otherwise.</returns>
        [HttpDelete("{shoppingItemId}")]
        public async Task<ActionResult> Delete(
            [BindRequired] [FromRoute] string shoppingItemId,
            CancellationToken cancellationToken
        )
        {
            var success = await this.shoppingItemService.DeleteAsync(
                this.User.Claims.RequiredId(),
                shoppingItemId,
                cancellationToken);
            return success ? this.Ok() : this.NotFound();
        }

        /// <summary>
        ///     Read all shopping items of the current user.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>A list of shopping items.</returns>
        [HttpGet]
        public async Task<IEnumerable<IShoppingItem>> Get(CancellationToken cancellationToken)
        {
            return await this.shoppingItemService.ReadAsync(
                this.User.Claims.RequiredId(),
                cancellationToken);
        }

        /// <summary>
        ///     Gets the shopping item with the specified id.
        /// </summary>
        /// <param name="shoppingItemId">The identifier of the shopping item.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The shopping item with the given id.</returns>
        [HttpGet("{shoppingItemId}")]
        public async Task<ActionResult<IShoppingItem>> Get(
            [BindRequired] [FromRoute] string shoppingItemId,
            CancellationToken cancellationToken
        )
        {
            var result = await this.shoppingItemService.ReadByIdAsync(
                this.User.Claims.RequiredId(),
                shoppingItemId,
                cancellationToken);
            if (result is null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        /// <summary>
        ///     Posts the specified shopping item.
        /// </summary>
        /// <param name="createShoppingItem">The shopping item to be created.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>The created shopping item.</returns>
        [HttpPost]
        public async Task<ActionResult> Post(
            [FromBody] CreateShoppingItem createShoppingItem,
            CancellationToken cancellationToken
        )
        {
            var shoppingItem = await this.shoppingItemService.CreateAsync(
                createShoppingItem,
                this.User.Claims.RequiredId(),
                cancellationToken);
            return this.CreatedAtAction(
                nameof(this.Get),
                new {shoppingItemId = shoppingItem.Id},
                shoppingItem);
        }

        /// <summary>
        ///     Update the specified shopping item.
        /// </summary>
        /// <param name="updateShoppingItem">The shopping item to be updated.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>An <see cref="OkResult" /> if the update succeeds and <see cref="NotFoundResult" /> otherwise.</returns>
        [HttpPut]
        public async Task<ActionResult> Put(
            [FromBody] UpdateShoppingItem updateShoppingItem,
            CancellationToken cancellationToken
        )
        {
            var success = await this.shoppingItemService.UpdateAsync(
                updateShoppingItem,
                this.User.Claims.RequiredId(),
                cancellationToken);
            return success ? this.Ok() : this.NotFound();
        }

        /// <summary>
        ///     Increase or decrease the quantity of the specified shopping item by the given amount.
        /// </summary>
        /// <param name="shoppingItemId">The shopping item to be updated.</param>
        /// <param name="amount">The quantity is increased or decreased by this amount.</param>
        /// <param name="operation">Specifies if it is a increase or decrease update.</param>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns>An <see cref="OkResult" /> if the update succeeds and <see cref="NotFoundResult" /> otherwise.</returns>
        [HttpPut("{operation}/{shoppingItemId}/{amount:int}")]
        public async Task<ActionResult> Put(
            [FromRoute]
            [BindRequired]
            [RegularExpression(
                $"^({ShoppingItemController.OperationIncrease}|{ShoppingItemController.OperationDecrease})$")]
            string operation,
            [FromRoute] [BindRequired] string shoppingItemId,
            [FromRoute]
            [BindRequired]
            [Range(
                1,
                CreateShoppingItem.MaxQuantity)]
            int amount,
            CancellationToken cancellationToken
        )
        {
            if (!Enum.TryParse(
                    operation,
                    true,
                    out UpdateOperation updateOperation) ||
                !Enum.IsDefined(updateOperation))
            {
                return this.BadRequest();
            }

            var success = await this.shoppingItemService.UpdateAsync(
                this.User.Claims.RequiredId(),
                shoppingItemId,
                updateOperation,
                amount,
                cancellationToken);

            return success ? this.Ok() : this.NotFound();
        }
    }
}
