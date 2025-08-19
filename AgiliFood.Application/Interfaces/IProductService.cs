using AgiliFood.Application.Dtos;

namespace AgiliFood.Application.Interfaces;

public interface IProductService
{

    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto> GetByIdAsync(long id);
    Task<ProductDto> CreateAsync(ProductDto productDto);
    Task<ProductDto> UpdateAsync(ProductDto productDto);
    Task<bool> DeleteAsync(long id);
}
