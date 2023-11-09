namespace Warehouse.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Extensions;
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
        ///     The business logic for handling shopping items.
        /// </summary>
        private readonly IDomainShoppingItemService shoppingItemService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ShoppingItemController" /> class.
        /// </summary>
        /// <param name="shoppingItemService">The business logic for handling shopping items.</param>
        public ShoppingItemController(IDomainShoppingItemService shoppingItemService)
        {
            this.shoppingItemService = shoppingItemService;
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

            return this.Ok(result);
        }
    }
}
