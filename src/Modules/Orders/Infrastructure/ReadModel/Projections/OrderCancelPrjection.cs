using Microsoft.EntityFrameworkCore;
using Modules.Orders.Domain.Events;
using SharedKernel.Domain;

namespace Modules.Orders.Infrastructure.ReadModel.Projections;

public sealed class OrderCancelledProjection : IDomainEventHandler<OrderCancelled>
{
    private readonly OrdersReadDbContext _db;

    public OrderCancelledProjection(OrdersReadDbContext db) => _db = db;

    public async Task HandleAsync(OrderCancelled @event, CancellationToken ct = default)
    {
        var o = await _db.Orders.SingleOrDefaultAsync(x => x.Id == @event.OrderId, ct);
        if (o is null) return;
        o.Status = "Cancelled";
        o.CancelledAt = @event.CancelledAtUtc;
        o.CancelReason = @event.Reason;
        await _db.SaveChangesAsync(ct);
    }
}