namespace Warehouse.Api.Auth
{
    using Generic.Base.Api.AuthServices.AuthService;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc cref="AuthControllerBase" />
    /// .
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : AuthControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthController" />
        ///     class.
        /// </summary>
        /// <param name="domainAuthService">The domain authentication service.</param>
        public AuthController(IDomainAuthService domainAuthService)
            : base(domainAuthService)
        {
        }
    }
}
