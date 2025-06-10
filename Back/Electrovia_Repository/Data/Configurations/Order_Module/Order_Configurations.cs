using Microsoft.EntityFrameworkCore;
using Electrovia_Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Electrovia_Repository.Data.Configurations.Order_Module
{
    public class Order_Configurations: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.Shipping_Address, Address => Address.WithOwner());
            
            builder.Property(p => p.Order_Status).HasConversion(s => s.ToString(),s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s));
           
            builder.Property(s => s.SubTotal).HasColumnType("decimal(18,2)");
            
            builder.HasMany(O => O.Order_Items).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
