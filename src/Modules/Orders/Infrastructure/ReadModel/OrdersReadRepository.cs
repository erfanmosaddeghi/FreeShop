using Mapster;
using Microsoft.EntityFrameworkCore;
using Modules.Orders.Application.DTOs;
using Modules.Orders.Application.Ports;

namespace Modules.Orders.Infrastructure.ReadModel;

public sealed class OrderReadRepository : IOrderReadRepository
{
    private readonly OrdersReadDbContext _db;

    public OrderReadRepository(OrdersReadDbContext db) => _db = db;

    public async Task<OrderDTO?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var read = await _db.Orders.Include(o => o.Lines).SingleOrDefaultAsync(o => o.Id == id, ct);
        return read?.Adapt<OrderDTO>();
    }

    public async Task<IReadOnlyList<OrderDTO>> GetListAsync(int skip, int take, CancellationToken ct)
    {
        var reads = await _db.Orders
            .OrderByDescending(o => o.Id)
            .Skip(skip)
            .Take(take)
            .Include(o => o.Lines)
            .ToListAsync(ct);

        return reads.Adapt<List<OrderDTO>>();
    }
}