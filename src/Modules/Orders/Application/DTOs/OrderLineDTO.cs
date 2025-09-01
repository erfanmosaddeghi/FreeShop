namespace Modules.Orders.Application.DTOs;

public sealed record OrderLineDTO(int LineNo, Guid ProductId, int Quantity, long UnitPriceRial, long LineTotalRial);