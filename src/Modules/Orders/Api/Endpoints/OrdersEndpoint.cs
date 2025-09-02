using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Orders.Api.Contracts;
using Modules.Orders.Application.Commands.AddLine;
using Modules.Orders.Application.Commands.CancelOrder;
using Modules.Orders.Application.Commands.PayOrder;
using Modules.Orders.Application.Commands.PlaceOrder;
using Modules.Orders.Application.Commands.RemoveLine;
using Modules.Orders.Application.Commands.UpdateLine;
using Modules.Orders.Application.Queries.GetOrderById;
using Modules.Orders.Application.Queries.GetOrdersList;

namespace Modules.Orders.Api.Endpoints;

public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder routes)
    {
        var g = routes.MapGroup("/api/orders");

        g.MapPost("/", async (PlaceOrderRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var cmd = new PlaceOrderCommand(req.CustomerId, req.Lines.Select(l => new PlaceOrderLine(l.ProductId, l.Quantity, l.UnitPriceRial)).ToList());
            var id = await mediator.Send(cmd, ct);
            return Results.Created($"/api/orders/{id}", new { id });
        });

        g.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var res = await mediator.Send(new GetOrderByIdQuery(id), ct);
            return res is null ? Results.NotFound() : Results.Ok(res);
        });

        g.MapGet("/", async (int? skip, int? take, IMediator mediator, CancellationToken ct) =>
        {
            var res = await mediator.Send(new GetOrdersListQuery(skip ?? 0, take ?? 50), ct);
            return Results.Ok(res);
        });

        g.MapPost("/{id:guid}/lines", async (Guid id, AddLineRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var ok = await mediator.Send(new AddLineCommand(id, req.ProductId, req.Quantity, req.UnitPriceRial), ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });

        g.MapPut("/{id:guid}/lines/{lineNo:int}", async (Guid id, int lineNo, UpdateLineRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var ok = await mediator.Send(new UpdateLineCommand(id, lineNo, req.Quantity, req.UnitPriceRial), ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });

        g.MapDelete("/{id:guid}/lines/{lineNo:int}", async (Guid id, int lineNo, IMediator mediator, CancellationToken ct) =>
        {
            var ok = await mediator.Send(new RemoveLineCommand(id, lineNo), ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });

        g.MapPost("/{id:guid}/pay", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var ok = await mediator.Send(new PayOrderCommand(id), ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });

        g.MapPost("/{id:guid}/cancel", async (Guid id, CancelOrderRequest req, IMediator mediator, CancellationToken ct) =>
        {
            var ok = await mediator.Send(new CancelOrderCommand(id, req.Reason), ct);
            return ok ? Results.NoContent() : Results.NotFound();
        });

        return routes;
    }
}