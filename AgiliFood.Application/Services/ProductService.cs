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
        var product = new Product(

            productDto.Name,
            productDto.Description,
            productDto.Brand,
            productDto.Flavor,
            productDto.Weight,
            productDto.Price,
            productDto.IsActive,
            productDto.BarCode,
            productDto.Image,
            productDto.ProductCategoryId,
            productDto.WeightUnit
        );

        _unitOfWork.ProductRepository.Create(product);

        await _unitOfWork.CommitAsync();

        productDto.Id = product.Id;

        return productDto;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == id);

        if (entity == null)
            return false;

        _unitOfWork.ProductRepository.Delete(entity);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _unitOfWork.ProductRepository.GetAllAsync();

        if (products == null || !products.Any())     
            return Enumerable.Empty<ProductDto>();

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Flavor = p.Flavor,
            Brand = p.Brand,
            Price = p.Price,
            IsActive = p.IsActive,
            BarCode = p.BarCode,
            ProductCategoryId = p.ProductCategoryId,
            Image = p.Image

        }).ToList();
    }

    public async Task<ProductDto> GetByIdAsync(long id)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(x => x.Id == id);

        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Flavor = product.Flavor,
            Brand = product.Brand,
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
            return null;

        product.SetName(productDto.Name);
        product.SetFlavor(productDto.Flavor);
        product.SetWeight(productDto.Weight, productDto.WeightUnit);
        product.ChangePrice(productDto.Price);
        product.SetDescription(productDto.Description);
        product.SetBrand(productDto.Brand);
        product.SetBarCode(productDto.BarCode);
        product.ChangeCategory(productDto.ProductCategoryId);
        product.ChangeImage(productDto.Image);

        if (productDto.IsActive)
            product.Activate();
        else
            product.Deactivate();

        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.CommitAsync();

        return productDto;
    }
}
