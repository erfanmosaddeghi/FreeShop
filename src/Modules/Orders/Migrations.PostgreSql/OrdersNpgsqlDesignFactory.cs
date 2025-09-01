using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Modules.Orders.Infrastructure.Persistence;

namespace Modules.Orders.Migrations.PostgreSql;

public sealed class OrdersNpgsqlDesignFactory : IDesignTimeDbContextFactory<OrdersDbContext>
{
    public OrdersDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var cfg = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var cs = cfg.GetConnectionString("OrdersNpgsql")
                 ?? "Host=localhost;Port=5432;Database=MyProject;Username=admin;Password=postgres";

        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseNpgsql(cs, b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory", "orders");
                b.MigrationsAssembly(typeof(OrdersNpgsqlDesignFactory).Assembly.FullName);
            })
            .Options;

        return new OrdersDbContext(options, dispatcher: null!);
    }
}