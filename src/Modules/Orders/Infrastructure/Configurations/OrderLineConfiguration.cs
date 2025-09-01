using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Orders.Domain.Entities;

namespace Modules.Orders.Infrastructure.Configurations;

public sealed class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> e)
    {
        e.ToTable($"TBL_{nameof(OrderLine).ToUpper()}", "orders");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).ValueGeneratedOnAdd();

        e.Property<Guid>("OrderId").IsRequired();
        e.Property(x => x.LineNo).IsRequired();
        e.Property(x => x.ProductId).IsRequired();
        e.Property(x => x.Quantity).IsRequired();
        e.Property(x => x.UnitPriceRial).IsRequired();

        e.HasIndex("OrderId", nameof(OrderLine.LineNo)).IsUnique();
    }
}