using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Orders.Application.Commands.PlaceOrder;
using Modules.Orders.Application.Interfaces;
using Modules.Orders.Application.Ports;
using Modules.Orders.Application.Validation;
using Modules.Orders.Infrastructure.Events;
using Modules.Orders.Infrastructure.Persistence;
using Modules.Orders.Infrastructure.Pipeline;
using Modules.Orders.Infrastructure.ReadModel;
using Modules.Orders.Infrastructure.ReadModel.Projections;
using Modules.Orders.Infrastructure.Repositories;
using SharedKernel.Domain;

namespace Modules.Orders.Infrastructure.Extensions;

public static class OrdersInfrastructureExtensions
{
    public static IServiceCollection AddOrdersInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        var provider = cfg["Db:Provider"]?.ToLowerInvariant() ?? "npgsql";

        if (provider == "sqlserver")
        {
            var cs = cfg.GetConnectionString("OrdersSqlServer")!;
            services.AddDbContext<OrdersDbContext>(o =>
                o.UseSqlServer(cs, b =>
                {
                    b.MigrationsHistoryTable("__EFMigrationsHistory", "orders");
                    b.MigrationsAssembly("Modules.Orders.Migrations.SqlServer");
                }));
            services.AddDbContext<OrdersReadDbContext>(o =>
                o.UseSqlServer(cs, b =>
                {
                    b.MigrationsHistoryTable("__EFMigrationsHistory", "orders_read");
                    b.MigrationsAssembly("Modules.Orders.Migrations.SqlServer");
                }));
        }
        else
        {
            var cs = cfg.GetConnectionString("OrdersNpgsql")!;
            services.AddDbContext<OrdersDbContext>(o =>
                o.UseNpgsql(cs, b =>
                {
                    b.MigrationsHistoryTable("__EFMigrationsHistory", "orders");
                    b.MigrationsAssembly("Modules.Orders.Migrations.PostgreSql");
                }));
            services.AddDbContext<OrdersReadDbContext>(o =>
                o.UseNpgsql(cs, b =>
                {
                    b.MigrationsHistoryTable("__EFMigrationsHistory", "orders_read");
                    b.MigrationsAssembly("Modules.Orders.Migrations.PostgreSql");
                }));
        }

        services.AddScoped<IOrdersRepository, EfOrdersRepository>();
        services.AddScoped<IOrderReadRepository, OrderReadRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IDomainEventDispatcher, InProcessDomainEventDispatcher>();

        services.AddMediatR(cfg2 => cfg2.RegisterServicesFromAssemblyContaining<PlaceOrderCommand>());

        services.AddValidatorsFromAssemblyContaining<PlaceOrderCommandValidator>();
        services.AddScoped<IDomainEventHandler<Modules.Orders.Domain.Events.OrderPlaced>, OrderPlacedProjection>();
        services.AddScoped<IDomainEventHandler<Modules.Orders.Domain.Events.OrderPaid>, OrderPaidProjection>();
        services.AddScoped<IDomainEventHandler<Modules.Orders.Domain.Events.OrderCancelled>, OrderCancelledProjection>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        return services;
    }
}