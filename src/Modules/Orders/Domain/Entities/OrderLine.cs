using SharedKernel.Domain;

namespace Modules.Orders.Domain.Entities;

public sealed class OrderLine : Entity<long>
{
    public int LineNo { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public long UnitPriceRial { get; private set; }

    private OrderLine() { }

    internal OrderLine(int lineNo, Guid productId, int quantity, long unitPriceRial)
    {
        if (lineNo <= 0) throw new ArgumentOutOfRangeException(nameof(lineNo));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (unitPriceRial < 0) throw new ArgumentOutOfRangeException(nameof(unitPriceRial));

        LineNo = lineNo;
        ProductId = productId;
        Quantity = quantity;
        UnitPriceRial = unitPriceRial;
    }

    public long LineTotalRial => checked(Quantity * UnitPriceRial);

    internal void UpdateQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        Quantity = quantity;
    }

    internal void UpdateUnitPrice(long unitPriceRial)
    {
        if (unitPriceRial < 0) throw new ArgumentOutOfRangeException(nameof(unitPriceRial));
        UnitPriceRial = unitPriceRial;
    }
}