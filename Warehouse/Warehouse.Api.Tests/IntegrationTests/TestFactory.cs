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

            var config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
            var jwtConfig = config.GetSection(JwtConfiguration.ConfigurationSection).Get<JwtConfiguration>();
            Assert.NotNull(jwtConfig);

            Environment.SetEnvironmentVariable(
                jwtConfig.KeyName,
                TestFactory.JwtKey);
            Environment.SetEnvironmentVariable(
                "X_API_KEY",
                TestFactory.ApiKey);

            const string mongoDbKey = "GENERIC_AUTH_SERVICES_MONGO";
            Environment.SetEnvironmentVariable(
                mongoDbKey,
                config.GetValue<string>(mongoDbKey));
        }
    }
}
