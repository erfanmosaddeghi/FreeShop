using Modules.Orders.Application.Abstractions.CQRS;
using Modules.Orders.Application.DTOs;

namespace Modules.Orders.Application.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(long Id) : IQuery<OrderDTO?>;