namespace AgiliFood.Application.Interfaces;

public interface IStockItemService
{
    Task<StockItem> GetByIdAsync(int id);

    Task<List<StockItem>> GetAllAsync();

}
