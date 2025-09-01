using MediatR;
using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.Interfaces;

namespace Modules.Orders.Application.Commands.RemoveLine;

public sealed class RemoveLineHandler : IRequestHandler<RemoveLineCommand, bool>
{
    private readonly IOrdersRepository _orders;

    public RemoveLineHandler(IOrdersRepository orders) => _orders = orders;

    public async Task<bool> Handle(RemoveLineCommand command, CancellationToken ct)
    {
        var order = await _orders.GetAsync(command.OrderId, ct);
        if (order is null) return false;

        order.RemoveLine(command.LineNo);
        return true;
    }
}