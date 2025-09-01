using Microsoft.EntityFrameworkCore;

namespace Modules.Orders.Infrastructure.ReadModel;

public sealed class OrdersReadDbContext : DbContext
{
    public DbSet<OrderRead> Orders => Set<OrderRead>();
    public DbSet<OrderLineRead> OrderLines => Set<OrderLineRead>();

    public OrdersReadDbContext(DbContextOptions<OrdersReadDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders_read");
        modelBuilder.Entity<OrderRead>(e =>
        {
            e.ToTable("orders");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id);
            e.Property(x => x.Status).HasMaxLength(16).IsRequired();
            e.Property(x => x.CancelReason).HasMaxLength(256);
            e.HasMany(x => x.Lines).WithOne().HasForeignKey(l => l.OrderId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderLineRead>(e =>
        {
            e.ToTable("order_lines");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id);
            e.Property(x => x.ProductId).IsRequired();
            e.HasIndex(x => new { x.OrderId, x.LineNo }).IsUnique();
        });
    }
}