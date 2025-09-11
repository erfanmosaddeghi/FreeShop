using Modules.Orders.Application.Abstractions.CQRS2;

namespace Modules.Orders.Application.Commands.AddLine;

public sealed record AddLineCommand(Guid OrderId, Guid ProductId, int Quantity, long UnitPriceRial) : ITransactionalCommand<bool>;