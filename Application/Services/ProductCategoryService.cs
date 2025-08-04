using AgiliFood.Api.Dtos;
using AgiliFood.Application.Interfaces;
using AgiliFood.Business.Interfaces;
using AgiliFood.Business.Models;
using System.Linq.Expressions;

namespace AgiliFood.Application.Services;

public class ProductCategoryService : IProductCategoryService
{
    private readonly IRepositoryBase<ProductCategory> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductCategoryService(IRepositoryBase<ProductCategory> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductCategoryDto>> GetAllAsync()
    {
        var entity = await _repository.GetAllAsync();

    }

    public async Task<ProductCategoryDto> GetByIdAsync(int id)
    {
        return await _repository.GetAsync(c => c.Id == id);
    }

    public async Task<ProductCategoryDto> CreateAsync(ProductCategory productCategory)
    {
        return _repository.Create(productCategory);
    }

    public async Task<ProductCategoryDto> UpdateAsync(ProductCategory productCategory)
    {
        return _repository.Update(productCategory);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var productCategory = await GetByIdAsync(id);
        if (productCategory == null) return false;
        _repository.Delete(productCategory);
        return true;
    }
    
}
