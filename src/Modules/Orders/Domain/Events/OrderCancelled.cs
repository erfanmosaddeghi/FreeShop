using SharedKernel.Domain;

namespace Modules.Orders.Domain.Events;

public sealed record OrderCancelled(
    Guid OrderId,
    string? Reason,
    DateTimeOffset CancelledAtUtc
) : IDomainEvent
{
    public DateTimeOffset OccurredAtUtc => CancelledAtUtc;
}