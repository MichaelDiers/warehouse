namespace Warehouse.Api.Models.Config
{
    using Warehouse.Api.Contracts.Config;

    /// <summary>
    ///     The configuration to create and validate json web tokens.
    /// </summary>
    public class JwtConfiguration : IJwtConfiguration
    {
        /// <summary>
        ///     Gets the audience for the token.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        ///     Gets the issuer of the token.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        ///     Gets the key to create and validate the token.
        /// </summary>
        public string Key { get; set; } = string.Empty;
    }
}
