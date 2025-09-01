
namespace Modules.Orders.Infrastructure.ReadModel;

public sealed class OrderLineRead
{
    public long Id { get; set; }
    public Guid OrderId { get; set; }
    public int LineNo { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public long UnitPriceRial { get; set; }
    public long LineTotalRial { get; set; }
}