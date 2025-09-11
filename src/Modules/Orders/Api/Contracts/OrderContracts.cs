namespace Modules.Orders.Api.Contracts;

public sealed record PlaceOrderRequest(long CustomerId, List<PlaceOrderLineRequest> Lines);
public sealed record PlaceOrderLineRequest(Guid ProductId, int Quantity, long UnitPriceRial);

public sealed record AddLineRequest(Guid ProductId, int Quantity, long UnitPriceRial);
public sealed record UpdateLineRequest(int? Quantity, long? UnitPriceRial);
public sealed record CancelOrderRequest(string? Reason);