using Warehouse.Api.Extensions;
using Warehouse.Api.Models.Config;

var source = new CancellationTokenSource(10000);

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
await builder.Services.AddWarehouseDb(
    appConfiguration.Warehouse,
    source.Token);

builder.Services.AddControllers();
//    options =>
//    {
//        options.Filters.Add()
//    })
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
