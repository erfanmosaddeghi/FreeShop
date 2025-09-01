using Modules.Orders.Domain.Entities;
using Modules.Orders.Domain.Enums;
using SharedKernel.Domain;

namespace Modules.Orders.Domain.Events;

public sealed record OrderPlaced(
    Guid OrderId,
    long CustomerId,
    DateTimeOffset OccurredAtUtc,
    IReadOnlyList<OrderLine> Lines) : IDomainEvent;