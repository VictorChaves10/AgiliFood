using AgiliFood.Business.Models.Product;
using AgiliFood.Business.Models.Stock;
using Microsoft.EntityFrameworkCore;

namespace AgiliFood.Data.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<StockItem> StockItems { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);


    }
}
