namespace Modules.Orders.Application.DTOs;

public sealed record OrderDTO(
    Guid Id,
    long CustomerId,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PaidAt,
    DateTimeOffset? CancelledAt,
    string? CancelReason,
    long TotalRial,
    IReadOnlyList<OrderLineDTO> Lines
);