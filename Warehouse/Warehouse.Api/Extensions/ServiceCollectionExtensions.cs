namespace Warehouse.Api.Extensions
{
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using MongoDB.Driver;
    using Warehouse.Api.Contracts;
    using Warehouse.Api.Contracts.Config;
    using Warehouse.Api.Contracts.ShoppingItems;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Providers;
    using Warehouse.Api.Services.Atomic;
    using Warehouse.Api.Services.Domain;

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
            IAppConfiguration configuration
        )
        {
            services.AddSingleton(_ => configuration);

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
            services.AddScoped<IAtomicStockItemService, AtomicStockItemService>();

            services.AddScoped<IShoppingItemProvider, ShoppingItemProvider>();
            services.AddScoped<IAtomicShoppingItemService, AtomicShoppingItemService>();

            services.AddScoped<IStockItemService, DomainStockItemService>();

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
            IJwtConfiguration configuration
        )
        {
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
                            ValidAudience = configuration.Audience,
                            ValidIssuer = configuration.Issuer,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key))
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
                        .RequireRole(CustomClaims.UserRole)
                        .RequireClaim(CustomClaims.IdClaim)
                        .RequireAssertion(ServiceCollectionExtensions.GuidAssertion)
                        .Build();
                });
        }

        /// <summary>
        ///     Adds the warehouse database.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The database configuration.</param>
        /// <returns>The given <paramref name="services" />.</returns>
        public static IServiceCollection AddWarehouseDb(
            this IServiceCollection services,
            IDatabaseConfiguration configuration
        )
        {
            services.AddSingleton<IMongoClient>(_ => new MongoClient(configuration.ConnectionString));

            return services;
        }

        /// <summary>
        ///     A assertion requirement for the id claim.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task" /> whose is true if the id claim is valid and false otherwise.</returns>
        private static Task<bool> GuidAssertion(AuthorizationHandlerContext context)
        {
            var idClaim = context.User.FindFirst(claim => claim.Type == CustomClaims.IdClaim);

            var result = idClaim != null &&
                         Guid.TryParse(
                             idClaim.Value,
                             out var guid) &&
                         guid != Guid.Empty;

            return Task.FromResult(result);
        }
    }
}
