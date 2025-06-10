using Electrovia_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Electrovia_Repository.Data.Configurations.Product_Module
{
    public class Product_Configurations : IEntityTypeConfiguration<Products>
    {
        public void Configure(EntityTypeBuilder<Products> builder)
        {
            builder.HasOne(P => P.ProductBrand).WithMany().HasForeignKey(P => P.ProductBrandId);
            builder.HasOne(P => P.ProductType).WithMany().HasForeignKey(P => P.ProductTypeId);
            builder.Property(P => P.Price).HasColumnType("decimal(18,2)");
            builder.Property(P => P.Name).HasMaxLength(100);
        }
    }
}
