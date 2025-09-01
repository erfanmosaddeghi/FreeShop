using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.Interfaces;


namespace Modules.Orders.Application.Commands.CancelOrder;

public sealed class CancelOrderHandler : ICommandHandler<CancelOrderCommand, bool>
{
    private readonly IOrdersRepository _orders;

    public CancelOrderHandler(IOrdersRepository orders) => _orders = orders;

    public async Task<bool> Handle(CancelOrderCommand command, CancellationToken ct)
    {
        var order = await _orders.GetAsync(command.OrderId, ct);
        if (order is null) return false;

        order.Cancel(command.Reason, DateTimeOffset.UtcNow);
        return true;
    }
}