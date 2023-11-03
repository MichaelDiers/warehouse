namespace Warehouse.Api.Tests.Utilities
{
    using System.Security.Claims;
    using System.Security.Principal;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Warehouse.Api.Contracts;

    internal static class ControllerContextService
    {
        public static ControllerContext Create(string userId)
        {
            var identity = new GenericIdentity(userId);
            identity.AddClaim(
                new Claim(
                    CustomClaims.IdClaim,
                    userId));

            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext {User = principal};

            return new ControllerContext {HttpContext = context};
        }
    }
}
