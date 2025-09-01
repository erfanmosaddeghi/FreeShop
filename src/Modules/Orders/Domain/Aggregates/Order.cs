using SharedKernel.Domain;
using Modules.Orders.Domain.Entities;
using Modules.Orders.Domain.Enums;
using Modules.Orders.Domain.Events;
using Modules.Orders.Domain.Exceptions;

namespace Modules.Orders.Domain.Aggregates;

public sealed class Order : AggregateRoot<long>
{
    public long CustomerId { get; private set; }

    private readonly List<OrderLine> _lines = new();
    public IReadOnlyList<OrderLine> Lines => _lines;

    public OrderStatus Status { get; private set; } = OrderStatus.Created;

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? PaidAt { get; private set; }
    public DateTimeOffset? CancelledAt { get; private set; }
    public string? CancelReason { get; private set; }

    private Order() { }

    private Order(long customerId, DateTimeOffset nowUtc)
    {
        if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));

        CustomerId = customerId;
        CreatedAt = nowUtc;
        Status = OrderStatus.Created;

        Raise(new OrderPlaced(0 /* filled by infra on save */, CustomerId, CreatedAt));
        // نکته: OrderId در ایونت بالا بعداً هنگام Publish می‌تواند با Id پر شود (پس از Persistence)
        // یا می‌توانی همین‌جا بعد از set Id در EF، ایونت را دوباره رفرنس دهی (الگوی متداول: Publish after SaveChanges).
    }

    public static Order Create(long customerId, DateTimeOffset nowUtc)
        => new(customerId, nowUtc);

    public void AddLine(Guid productId, int quantity, long unitPriceRial)
    {
        EnsureNotCancelled();

        var lineNo = _lines.Count == 0 ? 1 : _lines.Max(l => l.LineNo) + 1;
        var line = new OrderLine(lineNo, productId, quantity, unitPriceRial);
        _lines.Add(line);

        Touch(now: null);
    }

    public void RemoveLine(int lineNo)
    {
        EnsureNotCancelled();

        var line = _lines.FirstOrDefault(l => l.LineNo == lineNo);
        if (line is null) throw new DomainException($"Line {lineNo} not found.");

        _lines.Remove(line);
        Touch(now: null);
    }

    public void UpdateLine(int lineNo, int? quantity = null, long? unitPriceRial = null)
    {
        EnsureNotCancelled();

        var line = _lines.FirstOrDefault(l => l.LineNo == lineNo);
        if (line is null) throw new DomainException($"Line {lineNo} not found.");

        if (quantity.HasValue) line.UpdateQuantity(quantity.Value);
        if (unitPriceRial.HasValue) line.UpdateUnitPrice(unitPriceRial.Value);

        Touch(now: null);
    }

    public void MarkPaid(DateTimeOffset nowUtc)
    {
        EnsureNotCancelled();
        if (!_lines.Any()) throw new DomainException("Cannot pay an empty order.");
        if (Status == OrderStatus.Paid) return;

        Status = OrderStatus.Paid;
        PaidAt = nowUtc;
        UpdatedAt = nowUtc;

        Raise(new OrderPaid(Id, PaidAt.Value));
    }

    public void Cancel(string? reason, DateTimeOffset nowUtc)
    {
        if (Status == OrderStatus.Paid) throw new DomainException("Already paid.");
        if (Status == OrderStatus.Cancelled) return;

        Status = OrderStatus.Cancelled;
        CancelledAt = nowUtc;
        CancelReason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();
        UpdatedAt = nowUtc;

        Raise(new OrderCancelled(Id, CancelReason, CancelledAt.Value));
    }

    public long TotalRial => _lines.Sum(l => l.LineTotalRial);

    private void EnsureNotCancelled()
    {
        if (Status == OrderStatus.Cancelled) throw new DomainException("Order is cancelled.");
    }

    private void Touch(DateTimeOffset? now)
    {
        UpdatedAt = now ?? DateTimeOffset.UtcNow;
    }
}