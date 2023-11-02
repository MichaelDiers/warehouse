namespace Warehouse.Api.Contracts.Config
{
    /// <summary>
    ///     Describes the configuration of the application.
    /// </summary>
    public interface IAppConfiguration
    {
        /// <summary>
        ///     Gets the jwt configuration.
        /// </summary>
        IJwtConfiguration Jwt { get; }
    }
}
