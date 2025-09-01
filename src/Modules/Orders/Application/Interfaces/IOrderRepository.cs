using Modules.Orders.Domain.Aggregates;

namespace Modules.Orders.Application.Interfaces;

public interface IOrdersRepository
{
    Task<Order?> GetAsync(long id, CancellationToken ct);
    Task AddAsync(Order order, CancellationToken ct);
}