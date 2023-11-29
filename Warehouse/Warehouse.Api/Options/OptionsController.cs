namespace Warehouse.Api.Options
{
    using Generic.Base.Api.Controllers;
    using Generic.Base.Api.Models;
    using Microsoft.AspNetCore.Mvc;
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
                    nameof(StockItemController),
                    Urn.Options,
                    $"../{nameof(StockItemController)[..^10]}"))
        {
        }
    }
}
