using Modules.Orders.Api;
using Modules.Orders.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

OrderMappingConfig.RegisterMappings();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Ok(new { name = "Shop.Host", status = "up" }));

app.MapOrdersApi();
app.Run();
