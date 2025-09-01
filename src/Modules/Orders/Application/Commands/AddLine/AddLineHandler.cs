using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.Interfaces;

namespace Modules.Orders.Application.Commands.AddLine;

public sealed class AddLineHandler : ICommandHandler<AddLineCommand, bool>
{
    private readonly IOrdersRepository _orders;

    public AddLineHandler(IOrdersRepository orders) => _orders = orders;

    public async Task<bool> Handle(AddLineCommand command, CancellationToken ct)
    {
        var order = await _orders.GetAsync(command.OrderId, ct);
        if (order is null) return false;

        order.AddLine(command.ProductId, command.Quantity, command.UnitPriceRial);
        return true;
    }
}