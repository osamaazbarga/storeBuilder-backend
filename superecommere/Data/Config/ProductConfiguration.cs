using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using superecommere.Models.Products;

namespace superecommere.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<TblProducts>
    {
        public void Configure(EntityTypeBuilder<TblProducts> builder)
        {
            builder.Property(p=>p.Id).IsRequired();
            builder.Property(p=>p.Title).IsRequired().HasMaxLength(160);
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PictureUrl).IsRequired();
            builder.HasOne(b => b.ProductBrand).WithMany()
                .HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(b => b.ProductType).WithMany()
                .HasForeignKey(p => p.ProductTypeId);

        }
    }
}

