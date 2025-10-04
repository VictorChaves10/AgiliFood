using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgiliFood.Data.Mappings;

public class StockItemMapping : IEntityTypeConfiguration<StockItem>
{
    public void Configure(EntityTypeBuilder<StockItem> builder)
    {
        builder.ToTable("StockItems");

        builder.HasKey(si => si.Id);
        
        builder.Property(si => si.ProductId)
               .IsRequired();

        builder.Property(si => si.Quantity)
               .IsRequired();

        builder.Property(si => si.ExpirationDate);

        builder.Property(si => si.CreatedAt)
               .IsRequired();

        builder.HasOne(si => si.Product);

        builder.HasMany(si => si.Movements)
               .WithOne()
               .HasForeignKey("StockItemId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
