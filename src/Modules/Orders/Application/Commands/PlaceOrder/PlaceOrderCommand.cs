using Modules.Orders.Application.Abstractions.CQRS;

namespace Modules.Orders.Application.Commands.PlaceOrder;

public sealed record PlaceOrderLine(Guid ProductId, int Quantity, long UnitPriceRial);

public sealed record PlaceOrderCommand(
    long CustomerId,
    int Currency,
    IReadOnlyList<PlaceOrderLine> Lines
) : ICommand<long>;