using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Modules.Orders.Infrastructure.Persistence;

namespace Modules.Orders.Migrations.SqlServer;

public sealed class OrdersSqlServerDesignFactory : IDesignTimeDbContextFactory<OrdersDbContext>
{
    public OrdersDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var cfg = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var cs = cfg.GetConnectionString("OrdersSqlServer")
                 ?? "Server=localhost;Database=OrdersDb;User Id=sa;Password=Your_strong_password1!;TrustServerCertificate=True;";

        var options = new DbContextOptionsBuilder<OrdersDbContext>()
            .UseSqlServer(cs, b =>
            {
                b.MigrationsHistoryTable("__EFMigrationsHistory", "orders");
                b.MigrationsAssembly(typeof(OrdersSqlServerDesignFactory).Assembly.FullName);
            })
            .Options;

        return new OrdersDbContext(options, dispatcher: null!);
    }
}