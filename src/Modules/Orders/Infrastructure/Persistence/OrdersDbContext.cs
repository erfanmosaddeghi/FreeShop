using Microsoft.EntityFrameworkCore;
using Modules.Orders.Domain.Aggregates;
using Modules.Orders.Domain.Entities;
using SharedKernel.Domain;

namespace Modules.Orders.Infrastructure.Persistence;

public sealed class OrdersDbContext : DbContext
{
    private readonly IDomainEventDispatcher _dispatcher;

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    public OrdersDbContext(DbContextOptions<OrdersDbContext> options, IDomainEventDispatcher dispatcher)
        : base(options)
    {
        _dispatcher = dispatcher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);

        // var orderId = modelBuilder.Entity<Order>().Property(p => p.Id);

        // if (Database.IsNpgsql())
        // {
        //     orderId.UseIdentityByDefaultColumn();
        // }
        // else if (Database.IsSqlServer())
        // {
        //     orderId.UseIdentityColumn();
        // }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken); 
        return result;
    }
}