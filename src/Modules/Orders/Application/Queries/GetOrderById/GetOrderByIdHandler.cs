using Mapster;
using MediatR;
using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.DTOs;
using Modules.Orders.Application.Interfaces;


namespace Modules.Orders.Application.Queries.GetOrderById;

public sealed class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDTO?>
{
    private readonly IOrderReadRepository _orders;

    public GetOrderByIdHandler(IOrderReadRepository orders) => _orders = orders;

    public async Task<OrderDTO?> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var entity = await _orders.GetByIdAsync(query.Id, ct);
        return entity?.Adapt<OrderDTO>();
    }
}