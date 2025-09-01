using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Modules.Orders.Infrastructure.ReadModel;

namespace Modules.Orders.Migrations.PostgreSql;

public sealed class OrdersReadNpgsqlDesignFactory : IDesignTimeDbContextFactory<OrdersReadDbContext>
{
    public OrdersReadDbContext CreateDbContext(string[] args)
    {
        var cs = "Host=localhost;Port=5432;Database=MyProject;Username=admin;Password=postgres";

        var options = new DbContextOptionsBuilder<OrdersReadDbContext>()
            .UseNpgsql(cs, b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory", "orders_read");
                b.MigrationsAssembly(typeof(OrdersReadNpgsqlDesignFactory).Assembly.GetName().Name);
            })
            .Options;

        return new OrdersReadDbContext(options);
    }
}