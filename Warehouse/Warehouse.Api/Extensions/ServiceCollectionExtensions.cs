namespace Warehouse.Api.Extensions
{
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.Config;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Models.Config;
    using Warehouse.Api.Providers;
    using Warehouse.Api.Services;

    /// <summary>
    ///     Extensions for <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds the application configuration to the dependencies.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddAppConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddSingleton<IAppConfiguration>(
                _ => configuration.Get<AppConfiguration>() ?? throw new KeyNotFoundException(nameof(AppConfiguration)));

            return services;
        }

        /// <summary>
        ///     Adds custom information for swagger generator.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
        {
            return services.AddSwaggerGen(
                option =>
                {
                    option.SwaggerDoc(
                        "v1",
                        new OpenApiInfo
                        {
                            Title = "Demo API",
                            Version = "v1"
                        });
                    option.AddSecurityDefinition(
                        "Bearer",
                        new OpenApiSecurityScheme
                        {
                            In = ParameterLocation.Header,
                            Description = "Please enter a valid token",
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            BearerFormat = "JWT",
                            Scheme = "Bearer"
                        });
                    option.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                Array.Empty<string>()
                            }
                        });
                });
        }

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

        /// <summary>
        ///     Adds the jwt authentication to the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">AppConfiguration</exception>
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var appConfiguration = configuration.Get<AppConfiguration>();
            if (appConfiguration is null)
            {
                throw new KeyNotFoundException(nameof(AppConfiguration));
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidAudience = appConfiguration.Jwt.Audience,
                            ValidIssuer = appConfiguration.Jwt.Issuer,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfiguration.Jwt.Key))
                        };
                    });

            return services;
        }

        /// <summary>
        ///     Adds the jwt authorization.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
        {
            return services.AddAuthorization(
                options =>
                {
                    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                        .RequireRole(Roles.User)
                        .Build();
                });
        }
    }
}
