using Modules.Orders.Application.Ports;
using Modules.Orders.Infrastructure.Persistence;
using SharedKernel.Domain;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _db;
    private readonly IDomainEventDispatcher _dispatcher;

    public EfUnitOfWork(OrdersDbContext db, IDomainEventDispatcher dispatcher)
    {
        _db = db;
        _dispatcher = dispatcher;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // 1) Save write-side
        var result = await _db.SaveChangesAsync(ct);

        var events = _db.ChangeTracker
            .Entries<AggregateRoot<Guid>>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToArray();

        if (events.Length > 0)
            await _dispatcher.DispatchAsync(events, ct);

        return result;
    }
}