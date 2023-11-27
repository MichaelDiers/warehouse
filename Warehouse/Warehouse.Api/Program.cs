using Generic.Base.Api.HealthChecks;
using Generic.Base.Api.Jwt;
using Generic.Base.Api.Middleware.ApiKey;
using Generic.Base.Api.Middleware.ErrorHandling;
using Generic.Base.Api.MongoDb;
using Generic.Base.Api.MongoDb.AuthServices;
using Microsoft.OpenApi.Models;
using Warehouse.Api.ShoppingItems;
using Warehouse.Api.StockItems;

var source = new CancellationTokenSource(10000);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

// from Generic.Base.Api.MongoDb
builder.Services.AddHealthChecks();
builder.AddJwtTokenService();
builder.AddApiKey();
builder.AddAuthServices();

builder
    .AddUserBoundServices<CreateShoppingItem, ShoppingItem, UpdateShoppingItem, ResultShoppingItem, ShoppingItem,
        ShoppingItemTransformer, ShoppingItemDatabaseConfiguration>();
builder
    .AddUserBoundServices<CreateStockItem, StockItem, UpdateStockItem, ResultStockItem, StockItem, StockItemTransformer,
        StockItemDatabaseConfiguration>();

builder.Services.AddSwaggerGen(
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
        option.AddSecurityDefinition(
            "ApiKey",
            new OpenApiSecurityScheme
            {
                Description = "ApiKey is required",
                Type = SecuritySchemeType.ApiKey,
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
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
        option.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
    });

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// from Generic.Base.Api.MongoDb
app.MapCustomHealthChecks();
app.UseErrorHandling();
app.UseApiKey();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
