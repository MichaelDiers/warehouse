namespace Warehouse.Api.Auth
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.TokenService;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc cref="TokenEntryControllerBase" />
    [ApiController]
    [Route("api/[controller]")]
    public class TokenEntryController : TokenEntryControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="TokenEntryController" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        /// <param name="requiredClaims">The required claims for accessing the service.</param>
        public TokenEntryController(
            IDomainService<TokenEntry, TokenEntry, TokenEntry> domainService,
            IControllerTransformer<TokenEntry, ResultTokenEntry> transformer,
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
