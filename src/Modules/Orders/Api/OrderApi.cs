using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Modules.Orders.Api;

public static class OrdersApi
{
    public static WebApplication MapOrdersApi(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");
        group.MapGet("/ping", () => Results.Ok(new { ok = true, module = "orders" }));
        return app;
    }
}