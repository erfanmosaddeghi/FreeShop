using MediatR;
using Modules.Orders.Application.Interfaces;

namespace Modules.Orders.Application.Commands.PayOrder;

public sealed class PayOrderHandler : IRequestHandler<PayOrderCommand, bool>
{
    private readonly IOrdersRepository _orders;

    public PayOrderHandler(IOrdersRepository orders) => _orders = orders;

    public async Task<bool> Handle(PayOrderCommand command, CancellationToken ct)
    {
        var order = await _orders.GetAsync(command.OrderId, ct);
        if (order is null) return false;

        order.MarkPaid(DateTimeOffset.UtcNow);
        return true;
    }
}