using Modules.Orders.Application.Abstractions.CQRS2;

namespace Modules.Orders.Application.Commands.RemoveLine;

public sealed record RemoveLineCommand(Guid OrderId, int LineNo) : ITransactionalCommand<bool>;