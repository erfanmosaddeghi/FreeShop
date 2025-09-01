using Modules.Orders.Domain.Aggregates;

namespace Modules.Orders.Application.Interfaces;

public interface IOrdersRepository
{
    Task<Order?> GetAsync(Guid id, CancellationToken ct);
    Task AddAsync(Order order, CancellationToken ct);
}