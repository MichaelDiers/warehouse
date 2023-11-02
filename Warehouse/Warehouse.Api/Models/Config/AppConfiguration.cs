namespace Warehouse.Api.Models.Config
{
    using Warehouse.Api.Contracts.Config;

    /// <inheritdoc cref="IAppConfiguration" />
    public class AppConfiguration : IAppConfiguration
    {
        /// <summary>
        ///     Gets the jwt configuration.
        /// </summary>
        public JwtConfiguration Jwt { get; set; } = new();

        /// <summary>
        ///     Gets the jwt configuration.
        /// </summary>
        IJwtConfiguration IAppConfiguration.Jwt => this.Jwt;
    }
}
