using Microsoft.EntityFrameworkCore;
using Electrovia_Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Electrovia_Repository.Data.Configurations.Order_Module
{
    public class OrderItem_Configurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(o => o.Product_Order, product => product.WithOwner());

            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        }
    }
}
