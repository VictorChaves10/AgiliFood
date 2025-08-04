using AgiliFood.Business.Interfaces;
using AgiliFood.Data.Context;
using AgiliFood.Data.Repository;

namespace ClinicSoftware.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public ApplicationDbContext _context = context;

    private IProductRepository _productRepository;


    public IProductRepository ProductRepository
    {
        get
        {
            return _productRepository ??= new ProductRepository(_context);
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