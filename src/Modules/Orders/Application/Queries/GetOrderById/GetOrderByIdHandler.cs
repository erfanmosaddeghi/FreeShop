using Mapster;
using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.DTOs;
using Modules.Orders.Application.Interfaces;


namespace Modules.Orders.Application.Queries.GetOrderById;

public sealed class GetOrderByIdHandler : IQueryHandler<GetOrderByIdQuery, OrderDTO?>
{
    private readonly IOrdersRepository _orders;

    public GetOrderByIdHandler(IOrdersRepository orders) => _orders = orders;

    public async Task<OrderDTO?> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var entity = await _orders.GetAsync(query.Id, ct);
        return entity?.Adapt<OrderDTO>();
    }
}