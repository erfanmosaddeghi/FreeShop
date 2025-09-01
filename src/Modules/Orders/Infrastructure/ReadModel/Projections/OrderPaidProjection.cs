using Microsoft.EntityFrameworkCore;
using Modules.Orders.Domain.Events;
using SharedKernel.Domain;

namespace Modules.Orders.Infrastructure.ReadModel.Projections;

public sealed class OrderPaidProjection : IDomainEventHandler<OrderPaid>
{
    private readonly OrdersReadDbContext _db;

    public OrderPaidProjection(OrdersReadDbContext db) => _db = db;

    public async Task HandleAsync(OrderPaid @event, CancellationToken ct = default)
    {
        var o = await _db.Orders.SingleOrDefaultAsync(x => x.Id == @event.OrderId, ct);
        if (o is null) return;
        o.Status = "Paid";
        o.PaidAt = @event.PaidAtUtc;
        await _db.SaveChangesAsync(ct);
    }
}