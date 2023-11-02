namespace Warehouse.Api.Contracts.Config
{
    /// <summary>
    ///     The configuration to create and validate json web tokens.
    /// </summary>
    public interface IJwtConfiguration
    {
        /// <summary>
        ///     Gets the audience for the token.
        /// </summary>
        string Audience { get; }

        /// <summary>
        ///     Gets the issuer of the token.
        /// </summary>
        string Issuer { get; }

        /// <summary>
        ///     Gets the key to create and validate the token.
        /// </summary>
        string Key { get; }
    }
}
