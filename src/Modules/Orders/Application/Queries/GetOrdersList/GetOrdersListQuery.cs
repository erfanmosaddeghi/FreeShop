using MediatR;
using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.DTOs;

namespace Modules.Orders.Application.Queries.GetOrdersList;

public sealed record GetOrdersListQuery(int Skip = 0, int Take = 50) : IRequest<IReadOnlyList<OrderDTO>>;