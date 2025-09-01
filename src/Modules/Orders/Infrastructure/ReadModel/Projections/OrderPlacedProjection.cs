using Modules.Orders.Domain.Events;
using SharedKernel.Domain;

namespace Modules.Orders.Infrastructure.ReadModel.Projections;

public sealed class OrderPlacedProjection : IDomainEventHandler<OrderPlaced>
{
    private readonly OrdersReadDbContext _db;

    public OrderPlacedProjection(OrdersReadDbContext db) => _db = db;

    public async Task HandleAsync(OrderPlaced @event, CancellationToken ct = default)
    {
        var exists = await _db.Orders.FindAsync(new object[] { @event.OrderId }, ct);
        if (exists is not null) return;

        var read = new OrderRead
        {
            Id = @event.OrderId,
            CustomerId = @event.CustomerId,
            Status = "Created",
            CreatedAt = @event.OccurredAtUtc,
            TotalRial = @event.Lines.Sum(e => e.LineTotalRial),
            Lines = @event.Lines.Select(e => new OrderLineRead()
            {
                LineNo = e.LineNo,
                Id = e.Id,
                LineTotalRial = e.LineTotalRial,
                Quantity = e.Quantity,
                ProductId = e.ProductId
            }).ToList()
        };
        _db.Orders.Add(read);
        await _db.SaveChangesAsync(ct);
    }
}