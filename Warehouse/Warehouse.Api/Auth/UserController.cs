namespace Warehouse.Api.Auth
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.UserService;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc cref="UserControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : UserControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UserController" />
        ///     class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by the controller.</param>
        /// <param name="requiredClaims">The required claims for accessing the service.</param>
        public UserController(
            IDomainService<User, User, User> domainService,
            IControllerTransformer<User, ResultUser> transformer,
            IEnumerable<Claim> requiredClaims
        )
            : base(
                domainService,
                transformer,
                requiredClaims)
        {
        }
    }
}
