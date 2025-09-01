using Modules.Orders.Application.Abstractions.CQRS;

namespace Modules.Orders.Application.Commands.PayOrder;

public sealed record PayOrderCommand(long OrderId) : ICommand<bool>;