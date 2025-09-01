using Mapster;
using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.DTOs;
using Modules.Orders.Application.Interfaces;


namespace Modules.Orders.Application.Queries.GetOrdersList;

public sealed class GetOrdersListHandler : IQueryHandler<GetOrdersListQuery, IReadOnlyList<OrderDTO>>
{
    private readonly IOrderReadRepository _orders;

    public GetOrdersListHandler(IOrderReadRepository orders) => _orders = orders;

    public async Task<IReadOnlyList<OrderDTO>> Handle(GetOrdersListQuery query, CancellationToken ct)
    => await _orders.GetListAsync(query.Skip, query.Take, ct);
}