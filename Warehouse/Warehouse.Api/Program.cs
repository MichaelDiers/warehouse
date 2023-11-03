using Warehouse.Api.Extensions;
using Warehouse.Api.Models.Config;

var builder = WebApplication.CreateBuilder(args);

var appConfiguration = builder.Configuration.Get<AppConfiguration>();
if (appConfiguration is null)
{
    throw new KeyNotFoundException(nameof(AppConfiguration));
}

builder.Services.AddJwtAuthentication(appConfiguration.Jwt);
builder.Services.AddJwtAuthorization();

builder.Services.AddDependencies();
builder.Services.AddAppConfiguration(appConfiguration);
builder.Services.AddWarehouseDb(appConfiguration.Warehouse);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
