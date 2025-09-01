using Mapster;
using Modules.Orders.Application.DTOs;
using Modules.Orders.Domain.Aggregates;
using Modules.Orders.Domain.Entities;

namespace Modules.Orders.Application.Mapping;

public static class OrderMappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<OrderLine, OrderLineDTO>.NewConfig();

        TypeAdapterConfig<Order, OrderDTO>.NewConfig()
            .Map(dest => dest.Status, src => src.Status.ToString());
    }
}