using AgiliFood.Business.Interfaces;
using AgiliFood.Business.Models;
using AgiliFood.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AgiliFood.Data.Repository;

public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
{
    private readonly ApplicationDbContext _context;
    public ProductCategoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ProductCategory> GetAllWithProductsAsync(int id)
    {
        var entity = await _context.ProductCategories.AsNoTracking()
                                             .Include(pc => pc.Products)
                                             .FirstOrDefaultAsync(pc => pc.Id == id);

        return entity;
    }
}
