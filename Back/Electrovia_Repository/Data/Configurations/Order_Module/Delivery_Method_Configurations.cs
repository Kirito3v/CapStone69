using Electrovia_Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Electrovia_Repository.Data.Configurations.Order_Module
{
    public class Delivery_Method_Configurations : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(c => c.Cost).HasColumnType("decimal(18,2)");
        }
    }
}
