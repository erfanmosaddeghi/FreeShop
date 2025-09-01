using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.Interfaces;
using Modules.Orders.Domain.Aggregates;

namespace Modules.Orders.Application.Commands.PlaceOrder;

public sealed class PlaceOrderHandler : ICommandHandler<PlaceOrderCommand, long>
{
    private readonly IOrdersRepository _orders;

    public PlaceOrderHandler(IOrdersRepository orders) => _orders = orders;

    public async Task<long> Handle(PlaceOrderCommand command, CancellationToken ct)
    {
        if (command.Lines is null || command.Lines.Count == 0)
            throw new InvalidOperationException("Order lines are required.");

        var now = DateTimeOffset.UtcNow;
        var order = Order.Create(command.CustomerId, now);

        foreach (var l in command.Lines)
            order.AddLine(l.ProductId, l.Quantity, l.UnitPriceRial);

        await _orders.AddAsync(order, ct);
        return order.Id;
    }
}