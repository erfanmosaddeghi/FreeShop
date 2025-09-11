using Modules.Orders.Application.Abstractions.CQRS2;

namespace Modules.Orders.Application.Commands.PlaceOrder;

public sealed record PlaceOrderLine(Guid ProductId, int Quantity, long UnitPriceRial);

public sealed record PlaceOrderCommand(
    long CustomerId,
    IReadOnlyList<PlaceOrderLine> Lines
) : ITransactionalCommand<Guid>;