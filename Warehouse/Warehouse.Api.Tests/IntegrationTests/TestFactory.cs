namespace Warehouse.Api.Tests.IntegrationTests
{
    using Generic.Base.Api.Jwt;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Configuration;

    public class TestFactory : WebApplicationFactory<Program>
    {
        public const string ApiKey = "api key";
        public const string JwtKey = "Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx";

        /// <summary>
        ///     Gives a fixture an opportunity to configure the application before it gets built.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Hosting.IWebHostBuilder" /> for the application.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build()
                .GetSection(JwtConfiguration.ConfigurationSection)
                .Get<JwtConfiguration>();
            Assert.NotNull(config);

            Environment.SetEnvironmentVariable(
                config.KeyName,
                TestFactory.JwtKey);
            Environment.SetEnvironmentVariable(
                "X_API_KEY",
                TestFactory.ApiKey);
            Environment.SetEnvironmentVariable(
                "GENERIC_AUTH_SERVICES_MONGO",
                "mongodb://localhost:27017/?replicaSet=warehouse_replSet");
        }
    }
}
