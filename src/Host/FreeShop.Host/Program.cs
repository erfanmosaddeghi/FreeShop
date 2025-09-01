using Modules.Orders.Api;
using Modules.Orders.Application.Mapping;
using Modules.Orders.Application;
using Modules.Orders.Infrastructure.Extensions;
using Modules.Orders.Application.Abstractions.CQRS;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOrdersInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        if (ex is ValidationException vex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var details = new
            {
                type = "https://tools.ietf.org/html/rfc7807",
                title = "Validation failed",
                status = 400,
                errors = vex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };
            await context.Response.WriteAsJsonAsync(details);
            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var generic = new
        {
            type = "https://tools.ietf.org/html/rfc7807",
            title = "Unhandled error",
            status = 500
        };
        await context.Response.WriteAsJsonAsync(generic);
    });
});


OrderMappingConfig.RegisterMappings();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Ok(new { name = "Shop.Host", status = "up" }));

app.MapOrdersApi();
app.Run();
