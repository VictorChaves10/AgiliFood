
using AgiliFood.Application.Dtos;
using AgiliFood.Application.Interfaces;

namespace AgiliFood.Application.Services;

public class ProductCategoryService : IProductCategoryService
{
    public Task<ProductCategoryDto> CreateAsync(ProductCategoryDto productCategoryDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProductCategoryDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ProductCategoryDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductCategoryDto> UpdateAsync(ProductCategoryDto productCategoryDto)
    {
        throw new NotImplementedException();
    }
}
