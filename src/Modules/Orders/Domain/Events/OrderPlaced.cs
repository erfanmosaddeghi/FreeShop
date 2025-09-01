using Modules.Orders.Domain.Enums;
using SharedKernel.Domain;

namespace Modules.Orders.Domain.Events;

public sealed record OrderPlaced(
    long OrderId,
    long CustomerId,
    DateTimeOffset OccurredAtUtc
) : IDomainEvent;