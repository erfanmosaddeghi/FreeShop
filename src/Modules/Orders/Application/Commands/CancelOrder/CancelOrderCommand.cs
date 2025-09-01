using Modules.Orders.Application.Abstractions.CQRS;

namespace Modules.Orders.Application.Commands.CancelOrder;

public sealed record CancelOrderCommand(long OrderId, string? Reason) : ICommand<bool>;