namespace Modules.Orders.Infrastructure.ReadModel;

public sealed class OrderRead
{
    public Guid Id { get; set; }
    public long CustomerId { get; set; }
    public string Status { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? PaidAt { get; set; }
    public DateTimeOffset? CancelledAt { get; set; }
    public string? CancelReason { get; set; }
    public long TotalRial { get; set; }
    public List<OrderLineRead> Lines { get; set; } = new();
}