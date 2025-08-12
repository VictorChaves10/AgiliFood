
using AgiliFood.Application.Dtos;
using AgiliFood.Application.Interfaces;
using AgiliFood.Business.Interfaces;
using AgiliFood.Business.Models;

namespace AgiliFood.Application.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductCategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductCategoryDto> CreateAsync(ProductCategoryDto productCategoryDto)
    {
        var entity = new ProductCategory
        {
            Name = productCategoryDto.Name,
        };

        _unitOfWork.ProductCategoryRepository.Create(entity);
        await _unitOfWork.CommitAsync();

        return new ProductCategoryDto
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var productCategory = await _unitOfWork.ProductCategoryRepository.GetAsync(x => x.Id == id);

        if (productCategory == null)
            return false;

        _unitOfWork.ProductCategoryRepository.Delete(productCategory);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<IEnumerable<ProductCategoryDto>> GetAllAsync()
    {
        var productCategories = await _unitOfWork.ProductCategoryRepository.GetAllAsync();

        return productCategories.Select(x =>
        {
            var productCategoryDto = new ProductCategoryDto
            {
                Id = x.Id,
                Name = x.Name,
            };

            return productCategoryDto;
        });
    }

    public async Task<ProductCategoryDto> GetByIdAsync(int id)
    {
        var productCategory = await _unitOfWork.ProductCategoryRepository.GetAsync(x => x.Id == id);

        if (productCategory == null)
            return null;

        return new ProductCategoryDto
        {
            Id = productCategory.Id,
            Name = productCategory.Name,
        };
    }

    public async Task<ProductCategoryDto> GetAllWithProductsAsync(int id)
    {
        var entity = await _unitOfWork.ProductCategoryRepository.GetAllWithProductsAsync(id);
        
        if (entity == null)
            return null;
        
        var dto = new ProductCategoryDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Products = entity.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList()
        };

        return dto;
    }

    public async Task<ProductCategoryDto> UpdateAsync(ProductCategoryDto dto)
    {
        var entity = await _unitOfWork.ProductCategoryRepository
            .GetAsync(x => x.Id == dto.Id);

        if (entity == null)
            return null;

        entity.Name = dto.Name;

        _unitOfWork.ProductCategoryRepository.Update(entity);
        await _unitOfWork.CommitAsync();

        return new ProductCategoryDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

}
