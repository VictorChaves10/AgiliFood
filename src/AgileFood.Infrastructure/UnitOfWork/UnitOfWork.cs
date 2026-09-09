using AgileFood.Business.Exceptions;
using AgileFood.Business.Interfaces;
using AgileFood.Infrastructure.Context;
using AgileFood.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace AgileFood.Infrastructure.UnitOfWork;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private bool _disposed;

    private IProductRepository _productRepository;
    private IProductCategoryRepository _productCategoryRepository;
    private IStockItemRepository _stockItemRepository;
    private IUserRepository _userRepository;
    private IConsumptionRepository _consumptionRepository;
    private ICatalogItemRepository _catalogItemRepository;

    public IProductRepository ProductRepository
    {
        get { return _productRepository ??= new ProductRepository(_context); }
    }

    public IProductCategoryRepository ProductCategoryRepository
    {
        get { return _productCategoryRepository ??= new ProductCategoryRepository(_context); }
    }

    public IStockItemRepository StockItemRepository
    {
        get { return _stockItemRepository ??= new StockItemRepository(_context); }
    }

    public IUserRepository UserRepository
    {
        get { return _userRepository ??= new UserRepository(_context); }
    }

    public IConsumptionRepository ConsumptionRepository
    {
        get { return _consumptionRepository ??= new ConsumptionRepository(_context); }
    }

    public ICatalogItemRepository CatalogItemRepository
    {
        get { return _catalogItemRepository ??= new CatalogItemRepository(_context); }
    }

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyConflictException(
                "O registro foi alterado por outra operação simultânea. Tente novamente.");
        }
    }

    public async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await operation();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}
