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
        var result = await _db.SaveChangesAsync(ct);

        var aggregates = _db.ChangeTracker
                                         .Entries<AggregateRoot<Guid>>()
                                         .Select(e => e.Entity).ToList();
        
        var events = aggregates.SelectMany(e => e.DomainEvents).ToArray();
        
        foreach (var aggregate in aggregates) aggregate.ClearEvents(); // remove events
        
        if (events.Length > 0)
            await _dispatcher.DispatchAsync(events, ct);

        return result;
    }
}