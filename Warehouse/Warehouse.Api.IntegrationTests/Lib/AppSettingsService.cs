namespace Warehouse.Api.IntegrationTests.Lib
{
    using Microsoft.Extensions.Configuration;
    using Warehouse.Api.Contracts.Config;
    using Warehouse.Api.Models.Config;

    /// <summary>
    ///     Access the application configuration.
    /// </summary>
    internal class AppSettingsService
    {
        /// <summary>
        ///     The application configuration.
        /// </summary>
        private static IAppConfiguration? applicationConfiguration;

        /// <summary>
        ///     Gets the application configuration.
        /// </summary>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">ApplicationConfiguration</exception>
        public static IAppConfiguration ApplicationConfiguration
        {
            get
            {
                if (AppSettingsService.applicationConfiguration is null)
                {
                    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    AppSettingsService.applicationConfiguration = configuration.Get<AppConfiguration>();
                    if (AppSettingsService.applicationConfiguration is null)
                    {
                        throw new KeyNotFoundException(nameof(AppSettingsService.ApplicationConfiguration));
                    }
                }

                return AppSettingsService.applicationConfiguration;
            }
        }
    }
}
