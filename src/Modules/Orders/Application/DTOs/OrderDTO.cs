namespace Modules.Orders.Application.DTOs;

public sealed record OrderDTO(
    long Id,
    long CustomerId,
    string Currency,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PaidAt,
    DateTimeOffset? CancelledAt,
    string? CancelReason,
    long TotalRial,
    IReadOnlyList<OrderLineDTO> Lines
);