namespace Warehouse.Api.Auth
{
    using System.Security.Claims;
    using Generic.Base.Api.AuthServices.InvitationService;
    using Generic.Base.Api.Services;
    using Generic.Base.Api.Transformer;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc cref="InvitationControllerBase" />
    /// .
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationController : InvitationControllerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvitationController" /> class.
        /// </summary>
        /// <param name="domainService">The domain service.</param>
        /// <param name="transformer">Transformer for entries used by controllers.</param>
        /// <param name="requiredClaims">The required claims for accessing the service.</param>
        public InvitationController(
            IDomainService<Invitation, Invitation, Invitation> domainService,
            IControllerTransformer<Invitation, ResultInvitation> transformer,
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
