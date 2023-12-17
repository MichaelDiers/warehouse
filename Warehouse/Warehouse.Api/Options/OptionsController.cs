namespace Warehouse.Api.Options
{
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Mvc;
    using Warehouse.Api.Auth;
    using Warehouse.Api.StockItems;

    [ApiController]
    [Route("api/[controller]")]
    public class OptionsController : OptionsControllerBase
    {
        public OptionsController()
            : base(
                ClaimLink.Create(
                    nameof(OptionsController),
                    Urn.Options,
                    string.Empty),
                ClaimLink.Create(
                    nameof(AuthController),
                    Urn.Options,
                    $"../{nameof(AuthController)[..^10]}"),
                ClaimLink.Create(
                    nameof(InvitationController),
                    Urn.Options,
                    $"../{nameof(InvitationController)[..^10]}"),
                ClaimLink.Create(
                    nameof(TokenEntryController),
                    Urn.Options,
                    $"../{nameof(TokenEntryController)[..^10]}"),
                ClaimLink.Create(
                    nameof(UserController),
                    Urn.Options,
                    $"../{nameof(UserController)[..^10]}"),
                ClaimLink.Create(
                    nameof(StockItemController),
                    Urn.Options,
                    $"../{nameof(StockItemController)[..^10]}"))
        {
        }
    }
}
