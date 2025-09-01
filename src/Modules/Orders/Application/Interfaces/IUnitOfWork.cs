namespace Modules.Orders.Application.Ports;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}