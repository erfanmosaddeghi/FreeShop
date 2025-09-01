using Mapster;
using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.DTOs;
using Modules.Orders.Application.Interfaces;


namespace Modules.Orders.Application.Queries.GetOrdersList;

public sealed class GetOrdersListHandler : IQueryHandler<GetOrdersListQuery, IReadOnlyList<OrderDTO>>
{
    private readonly IOrdersRepository _orders;

    public GetOrdersListHandler(IOrdersRepository orders) => _orders = orders;

    public async Task<IReadOnlyList<OrderDTO>> Handle(GetOrdersListQuery query, CancellationToken ct)
    {
        var list = new List<OrderDTO>();
        for (var i = 0; i < query.Take; i++)
        {
            var id = query.Skip + 1 + i;
            var entity = await _orders.GetAsync(id, ct);
            if (entity is not null) list.Add(entity.Adapt<OrderDTO>());
        }
        return list;
    }
}