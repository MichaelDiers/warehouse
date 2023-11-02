namespace Warehouse.Api.Extensions
{
    using System.Security.Claims;
    using Warehouse.Api.Contracts;

    /// <summary>
    ///     Extensions for <see cref="IEnumerable{T}" /> of <see cref="Claim" />.
    /// </summary>
    public static class ClaimsExtensions
    {
        /// <summary>
        ///     Read the value of the required id claim.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns>The value of the id claim.</returns>
        public static string RequiredId(this IEnumerable<Claim> claims)
        {
            return claims.First(claim => claim.Type == CustomClaims.IdClaim).Value;
        }
    }
}
