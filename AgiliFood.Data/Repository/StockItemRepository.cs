using AgiliFood.Business.Interfaces;
using AgiliFood.Data.Context;

namespace AgiliFood.Data.Repository;

public class StockItemRepository : IStockItemRepository
{
    private readonly ApplicationDbContext _context;

    public StockItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }



}
