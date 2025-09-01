using SharedKernel.Domain;

namespace Modules.Orders.Domain.Events;

public sealed record OrderPaid(
    Guid OrderId,
    DateTimeOffset PaidAtUtc
) : IDomainEvent
{
    public DateTimeOffset OccurredAtUtc => PaidAtUtc;
}