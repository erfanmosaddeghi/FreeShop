using Modules.Orders.Application.DTOs;

public interface IOrderReadRepository
{
    Task<OrderDTO?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<OrderDTO>> GetListAsync(int skip, int take, CancellationToken ct);
}