using AgiliFood.Business.Interfaces;
using AgiliFood.Data.Context;
using AgiliFood.Data.Repository;

namespace ClinicSoftware.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    private IProductRepository _productRepository;

    private IProductCategoryRepository _productCategoryRepository;


    public IProductRepository ProductRepository
    {
        get
        {
            return _productRepository ??= new ProductRepository(_context);
        }
    }

    public IProductCategoryRepository ProductCategoryRepository
    {
        get
        {
            return _productCategoryRepository ??= new ProductCategoryRepository(_context);
        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}