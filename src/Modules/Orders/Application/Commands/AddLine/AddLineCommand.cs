using Modules.Orders.Application.Abstractions.CQRS;

namespace Modules.Orders.Application.Commands.AddLine;

public sealed record AddLineCommand(long OrderId, Guid ProductId, int Quantity, long UnitPriceRial) : ICommand<bool>;