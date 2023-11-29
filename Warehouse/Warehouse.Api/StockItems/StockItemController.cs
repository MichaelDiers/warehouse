namespace Warehouse.Api.StockItems
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///     The controller for manipulating stock items.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(Role.Accessor))]
    [Authorize(Roles = nameof(Role.User))]
    public class StockItemController
        : UserBoundCrudController<CreateStockItem, StockItem, UpdateStockItem, ResultStockItem>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Generic.Base.Api.Controllers.CrudController`4" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        public StockItemController(
            IUserBoundDomainService<CreateStockItem, StockItem, UpdateStockItem> domainService,
            IControllerTransformer<StockItem, ResultStockItem> transformer
        )
            : base(
                domainService,
                transformer,
                new[]
                {
                    new Claim(
                        ClaimTypes.Role,
                        Role.User.ToString()),
                    new Claim(
                        ClaimTypes.Role,
                        Role.Accessor.ToString())
                })
        {
        }

        /// <summary>
        ///     Determines whether the specified identifier is valid.
        /// </summary>
        /// <param name="id">The identifier to be checked.</param>
        /// <returns>
        ///     <c>true</c> if the specified identifier is valid; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsIdValid(string id)
        {
            return Guid.TryParse(
                       id,
                       out var guid) &&
                   guid != Guid.Empty;
        }
    }
}
