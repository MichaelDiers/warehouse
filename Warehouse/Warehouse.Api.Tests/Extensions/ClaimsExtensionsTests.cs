namespace Warehouse.Api.Tests.Extensions
{
    using System.Security.Claims;
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Tests.Utilities;

    /// <summary>
    ///     Tests for <see cref="ClaimsExtensions" />.
    /// </summary>
    [Trait(
        Constants.TraitType,
        Constants.TraitValueUnitTest)]
    public class ClaimsExtensionsTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(
            false,
            ClaimTypes.Name,
            ClaimTypes.Actor)]
        [InlineData(
            true,
            ClaimTypes.Name,
            CustomClaims.IdClaim)]
        public void RequiredIdFails(bool success, params string[] claimTypes)
        {
            var claims = claimTypes.Select(
                    ct => new Claim(
                        ct,
                        Guid.NewGuid().ToString()))
                .ToArray();

            if (success)
            {
                var id = claims.RequiredId();
                Assert.Equal(
                    claims.First(x => x.Type == CustomClaims.IdClaim).Value,
                    id);
            }
            else
            {
                Assert.Throws<InvalidOperationException>(() => claims.RequiredId());
            }
        }
    }
}
