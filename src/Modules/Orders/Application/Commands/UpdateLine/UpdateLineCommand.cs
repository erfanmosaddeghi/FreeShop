using Modules.Orders.Application.Abstractions.CQRS;

namespace Modules.Orders.Application.Commands.UpdateLine;

public sealed record UpdateLineCommand(long OrderId, int LineNo, int? Quantity, long? UnitPriceRial) : ICommand<bool>;