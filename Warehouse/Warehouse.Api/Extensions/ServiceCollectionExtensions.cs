namespace Warehouse.Api.Extensions
{
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Providers;
    using Warehouse.Api.Services;

    /// <summary>
    ///     Extensions for <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the dependencies that are injected.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IStockItemProvider, StockItemProvider>();
            services.AddScoped<IStockItemService, StockItemService>();

            return services;
        }
    }
}
