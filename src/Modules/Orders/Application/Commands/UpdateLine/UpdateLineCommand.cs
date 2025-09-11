using Modules.Orders.Application.Abstractions.CQRS2;

namespace Modules.Orders.Application.Commands.UpdateLine;

public sealed record UpdateLineCommand(Guid OrderId, int LineNo, int? Quantity, long? UnitPriceRial) : ITransactionalCommand<bool>;