using Modules.Orders.Application.Abstractions.CQRS;

namespace Modules.Orders.Application.Commands.RemoveLine;

public sealed record RemoveLineCommand(long OrderId, int LineNo) : ICommand<bool>;