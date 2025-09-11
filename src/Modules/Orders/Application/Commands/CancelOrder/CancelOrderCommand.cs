using Modules.Orders.Application.Abstractions.CQRS2;

namespace Modules.Orders.Application.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid OrderId, string? Reason) : ITransactionalCommand<bool>;