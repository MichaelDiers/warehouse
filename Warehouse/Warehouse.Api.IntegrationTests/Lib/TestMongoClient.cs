namespace Warehouse.Api.IntegrationTests.Lib
{
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using Warehouse.Api.Models.StockItems;

    internal static class TestMongoClient
    {
        public static IMongoDatabase Connect(IServiceCollection? services = null)
        {
            var configuration = AppSettingsService.ApplicationConfiguration;

            var client = new MongoClient(configuration.Warehouse.ConnectionString);
            if (services is not null)
            {
                services.AddSingleton<IMongoClient>(client);
                services.AddTransient<IMongoDatabase>(_ => client.GetDatabase(configuration.Warehouse.DatabaseName));
            }

            return client.GetDatabase(configuration.Warehouse.DatabaseName);
        }

        public static IMongoCollection<DatabaseStockItem> StockItemCollection(IServiceCollection? services = null)
        {
            return TestMongoClient.Connect(services).GetCollection<DatabaseStockItem>(DatabaseStockItem.CollectionName);
        }
    }
}
