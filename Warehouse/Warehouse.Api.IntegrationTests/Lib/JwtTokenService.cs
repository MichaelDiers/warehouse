namespace Warehouse.Api.IntegrationTests.Lib
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;
    using Warehouse.Api.Contracts;

    /// <summary>
    ///     A service for creating json web tokens.
    /// </summary>
    internal class JwtTokenService
    {
        /// <summary>
        ///     Creates the token.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The token as a string.</returns>
        public string CreateToken(string id)
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Role,
                    CustomClaims.UserRole),
                new Claim(
                    CustomClaims.IdClaim,
                    id)
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppSettingsService.ApplicationConfiguration.Jwt.Key));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials,
                audience: AppSettingsService.ApplicationConfiguration.Jwt.Audience,
                issuer: AppSettingsService.ApplicationConfiguration.Jwt.Issuer);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
