using AgiliFood.Api.Dtos;

namespace AgiliFood.Application.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategoryDto>> GetAllAsync();
        Task<ProductCategoryDto> GetByIdAsync(int id);
        Task<ProductCategoryDto> CreateAsync(ProductCategoryDto productCategoryDto);
        Task<ProductCategoryDto> UpdateAsync(ProductCategoryDto productCategoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
