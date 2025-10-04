using AgiliFood.Application.Interfaces;
using AgiliFood.Business.Interfaces;

namespace AgiliFood.Application.Services;

public class StockItemService : IStockItemService
{
    private readonly IUnitOfWork _unitOfWork;

    public StockItemService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<List<StockItem>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<StockItem> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
