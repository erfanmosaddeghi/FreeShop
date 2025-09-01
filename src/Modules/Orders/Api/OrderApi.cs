using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Modules.Orders.Api.Endpoints;

namespace Modules.Orders.Api;

public static class OrdersApi
{
    public static WebApplication MapOrdersApi(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");
        group.MapOrdersEndpoints();
        return app;
    }
}