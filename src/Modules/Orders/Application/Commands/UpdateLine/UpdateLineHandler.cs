using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.Interfaces;

namespace Modules.Orders.Application.Commands.UpdateLine;

public sealed class UpdateLineHandler : ICommandHandler<UpdateLineCommand, bool>
{
    private readonly IOrdersRepository _orders;

    public UpdateLineHandler(IOrdersRepository orders) => _orders = orders;

    public async Task<bool> Handle(UpdateLineCommand command, CancellationToken ct)
    {
        var order = await _orders.GetAsync(command.OrderId, ct);
        if (order is null) return false;

        order.UpdateLine(command.LineNo, command.Quantity, command.UnitPriceRial);
        return true;
    }
}