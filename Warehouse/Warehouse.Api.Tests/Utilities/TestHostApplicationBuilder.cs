namespace Warehouse.Api.Tests.Utilities
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MongoDB.Driver;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.Config;

    internal class TestHostApplicationBuilder
    {
        /// <summary>
        ///     Initialize the host and get a certain service.
        /// </summary>
        /// <typeparam name="T">The type of the service to be requested from the host.</typeparam>
        /// <param name="addServices">The add services dependencies.</param>
        /// <returns>The requested service.</returns>
        public static T GetService<T>(params Func<IServiceCollection, IServiceCollection>[] addServices)
        {
            var builder = Host.CreateApplicationBuilder();
            foreach (var addService in addServices)
            {
                addService(builder.Services);
            }

            var host = builder.Build();
            var service = host.Services.GetService<T>();

            Assert.NotNull(service);

            return service;
        }

        /// <summary>
        ///     Initialize the host and get a certain service.
        /// </summary>
        /// <typeparam name="T">The type of the service to be requested from the host.</typeparam>
        /// <typeparam name="TIReplace1">The interface type of the service1 to be replaced.</typeparam>
        /// <param name="addServices">The add services dependencies.</param>
        /// <param name="replace1">The replacement service1.</param>
        /// <returns>The requested service.</returns>
        public static T GetService<T, TIReplace1>(
            IEnumerable<Func<IServiceCollection, IServiceCollection>> addServices,
            TIReplace1 replace1
        ) where TIReplace1 : class
        {
            var builder = TestHostApplicationBuilder.InitHostApplicationBuilder(
                addServices,
                new[] {typeof(TIReplace1)});

            Host.CreateApplicationBuilder();

            builder.Services.AddScoped<TIReplace1>(_ => replace1);

            return TestHostApplicationBuilder.Build<T>(builder);
        }

        /// <summary>
        ///     Initialize the host and get a certain service.
        /// </summary>
        /// <typeparam name="T">The type of the service to be requested from the host.</typeparam>
        /// <typeparam name="TIReplace1">The interface type of the service1 to be replaced.</typeparam>
        /// <typeparam name="TIReplace2">The interface type of the service2 to be replaced.</typeparam>
        /// <param name="addServices">The add services dependencies.</param>
        /// <param name="replace1">The replacement service1.</param>
        /// <param name="replace2">The replacement service2.</param>
        /// <returns>The requested service.</returns>
        public static T GetService<T, TIReplace1, TIReplace2>(
            IEnumerable<Func<IServiceCollection, IServiceCollection>> addServices,
            TIReplace1 replace1,
            TIReplace2 replace2
        ) where TIReplace1 : class where TIReplace2 : class
        {
            var builder = TestHostApplicationBuilder.InitHostApplicationBuilder(
                addServices,
                new[]
                {
                    typeof(TIReplace1),
                    typeof(TIReplace2)
                });

            Host.CreateApplicationBuilder();

            builder.Services.AddScoped<TIReplace1>(_ => replace1);
            builder.Services.AddScoped<TIReplace2>(_ => replace2);

            return TestHostApplicationBuilder.Build<T>(builder);
        }

        /// <summary>
        ///     Initialize the host and get a certain service.
        /// </summary>
        /// <typeparam name="T">The type of the service to be requested from the host.</typeparam>
        /// <typeparam name="TIReplace1">The interface type of the service1 to be replaced.</typeparam>
        /// <typeparam name="TIReplace2">The interface type of the service2 to be replaced.</typeparam>
        /// <typeparam name="TIReplace3">The interface type of the service3 to be replaced.</typeparam>
        /// <param name="addServices">The add services dependencies.</param>
        /// <param name="replace1">The replacement service1.</param>
        /// <param name="replace2">The replacement service2.</param>
        /// <param name="replace3">The replacement service3.</param>
        /// <returns>The requested service.</returns>
        public static T GetService<T, TIReplace1, TIReplace2, TIReplace3>(
            IEnumerable<Func<IServiceCollection, IServiceCollection>> addServices,
            TIReplace1 replace1,
            TIReplace2 replace2,
            TIReplace3 replace3
        ) where TIReplace1 : class where TIReplace2 : class where TIReplace3 : class
        {
            var builder = TestHostApplicationBuilder.InitHostApplicationBuilder(
                addServices,
                new[]
                {
                    typeof(TIReplace1),
                    typeof(TIReplace2),
                    typeof(TIReplace3)
                });

            Host.CreateApplicationBuilder();

            builder.Services.AddScoped<TIReplace1>(_ => replace1);
            builder.Services.AddScoped<TIReplace2>(_ => replace2);
            builder.Services.AddScoped<TIReplace3>(_ => replace3);

            return TestHostApplicationBuilder.Build<T>(builder);
        }

        /// <summary>
        ///     Initialize the host and get a certain type of services.
        /// </summary>
        /// <typeparam name="TA">The type of the first service.</typeparam>
        /// <typeparam name="TB">The type of the second service.</typeparam>
        /// <param name="addServices">The add services dependencies.</param>
        /// <returns>The requested services.</returns>
        public static async Task<(TA, TB)> GetServices<TA, TB>(
            params Func<IServiceCollection, IServiceCollection>[] addServices
        )
        {
            var builder = Host.CreateApplicationBuilder();
            foreach (var addService in addServices)
            {
                addService(builder.Services);
            }

            if (typeof(TA) == typeof(IMongoClient) || typeof(TB) == typeof(IMongoClient))
            {
                var appConfiguration = builder.Configuration.Get<AppConfiguration>();
                Assert.NotNull(appConfiguration);
                await builder.Services.AddWarehouseDb(
                    appConfiguration.Warehouse,
                    new CancellationToken());
            }

            var host = builder.Build();
            var serviceA = host.Services.GetService<TA>();
            var serviceB = host.Services.GetService<TB>();

            Assert.NotNull(serviceA);
            Assert.NotNull(serviceB);

            return (serviceA, serviceB);
        }

        private static T Build<T>(HostApplicationBuilder builder)
        {
            var host = builder.Build();
            var service = host.Services.GetService<T>();

            Assert.NotNull(service);

            return service;
        }

        private static HostApplicationBuilder InitHostApplicationBuilder(
            IEnumerable<Func<IServiceCollection, IServiceCollection>> addServices,
            IEnumerable<Type> toBeRemoved
        )
        {
            var builder = Host.CreateApplicationBuilder();
            foreach (var addService in addServices)
            {
                addService(builder.Services);
            }

            var obsoleteServices = builder.Services.Where(
                    service => toBeRemoved.Any(type => type == service.ServiceType))
                .ToArray();
            foreach (var serviceDescriptor in obsoleteServices)
            {
                builder.Services.Remove(serviceDescriptor);
            }

            return builder;
        }
    }
}
