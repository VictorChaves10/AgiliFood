using AgiliFood.Business.Interfaces;
using AgiliFood.Business.Models.Product;
using AgiliFood.Data.Context;

namespace AgiliFood.Data.Repository;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {

    }
}

