using AgiliFood.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgiliFood.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Description)
               .HasMaxLength(500);

        builder.Property(p => p.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.IsActive)
               .IsRequired();

        builder.Property(p => p.Weight)
               .IsRequired();

        builder.Property(p => p.WeightUnit)
                .IsRequired();

        builder.Property(p => p.BarCode)
               .HasMaxLength(50);

        builder.Property(p => p.ProductCategoryId)
               .IsRequired();

        builder.Property(p => p.Flavor)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Brand)
               .HasMaxLength(100);

        builder.Property(p => p.Image)
               .HasMaxLength(200);

        builder.HasOne(p => p.ProductCategory)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.ProductCategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }

}
