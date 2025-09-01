using Microsoft.EntityFrameworkCore;
using Modules.Orders.Application.Interfaces;
using Modules.Orders.Application.Ports;
using Modules.Orders.Domain.Aggregates;
using Modules.Orders.Infrastructure.Persistence;

namespace Modules.Orders.Infrastructure.Repositories;

public sealed class EfOrdersRepository : IOrdersRepository
{
    private readonly OrdersDbContext _db;

    public EfOrdersRepository(OrdersDbContext db)
    {
        _db = db;
    }

    public async Task<Order?> GetAsync(Guid id, CancellationToken ct)
    {
        return await _db.Orders
            .Include(o => o.Lines)
            .SingleOrDefaultAsync(o => o.Id == id, ct);
    }

    public async Task AddAsync(Order order, CancellationToken ct)
    {
        await _db.Orders.AddAsync(order, ct);
    }
}