using Modules.Orders.Application.Abstractions.CQRS2;

namespace Modules.Orders.Application.Commands.PayOrder;

public sealed record PayOrderCommand(Guid OrderId) : ITransactionalCommand<bool>;