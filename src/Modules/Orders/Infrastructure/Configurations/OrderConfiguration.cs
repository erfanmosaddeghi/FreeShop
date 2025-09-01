using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Orders.Domain.Aggregates;
using Modules.Orders.Domain.Enums;

namespace Modules.Orders.Infrastructure.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> e)
    {
        e.ToTable($"TBL_{nameof(Order).ToUpper()}", "orders");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).ValueGeneratedOnAdd();

        e.Property(x => x.CustomerId).IsRequired();
        e.Property(x => x.Status).HasConversion<string>().HasMaxLength(16).IsRequired();

        e.Property(x => x.CreatedAt).IsRequired();
        e.Property(x => x.UpdatedAt);
        e.Property(x => x.PaidAt);
        e.Property(x => x.CancelledAt);
        e.Property(x => x.CancelReason).HasMaxLength(256);

        e.Navigation(x => x.Lines).UsePropertyAccessMode(PropertyAccessMode.Field);
        e.HasMany(x => x.Lines).WithOne().HasForeignKey("OrderId").OnDelete(DeleteBehavior.Cascade);
    }
}