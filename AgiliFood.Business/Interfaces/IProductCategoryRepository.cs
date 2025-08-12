using AgiliFood.Business.Models;

namespace AgiliFood.Business.Interfaces
{
    public interface IProductCategoryRepository : IRepositoryBase<ProductCategory>
    {
        Task<ProductCategory> GetAllWithProductsAsync(int id);
    }
}
