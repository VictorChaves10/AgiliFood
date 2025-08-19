using AgiliFood.Application.Dtos;
using AgiliFood.Application.Interfaces;
using AgiliFood.Business.Interfaces;
using AgiliFood.Business.Models;

namespace AgiliFood.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto> CreateAsync(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            IsActive = productDto.IsActive,
            BarCode = productDto.BarCode,
            ProductCategoryId = productDto.ProductCategoryId,
            Image = productDto.Image,

        };

        _unitOfWork.ProductRepository.Create(product);

        await _unitOfWork.CommitAsync();

        productDto.Id = product.Id;

        return productDto;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == id);
        if (entity == null)
        {
            throw new Exception("Product not found.");
        }

        _unitOfWork.ProductRepository.Delete(entity);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync();

        if (products == null || !products.Any())
        {
            throw new Exception("No products found.");
        }

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            IsActive = p.IsActive,
            BarCode = p.BarCode

        }).ToList();
    }

    public async Task<ProductDto> GetByIdAsync(long id)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == id);

        if (product == null)
        {
            throw new Exception($"Product with ID {id} not found.");
        }

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            IsActive = product.IsActive,
            BarCode = product.BarCode,
            ProductCategoryId = product.ProductCategoryId,
            Image = product.Image
        };
    }

    public async Task<ProductDto> UpdateAsync(ProductDto productDto)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == productDto.Id);

        if (product == null)
        {
            throw new Exception($"Product with ID {productDto.Id} not found.");
        }

        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.IsActive = productDto.IsActive;
        product.BarCode = productDto.BarCode;
        product.ProductCategoryId = productDto.ProductCategoryId;
        product.Image = productDto.Image;

        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.CommitAsync();

        return productDto;
    }
}
