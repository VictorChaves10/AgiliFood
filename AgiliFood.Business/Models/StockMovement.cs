namespace AgiliFood.Business.Models;

public class StockMovement
{
    public long Id { get; private set; }

    public long StockItemId { get; private set; }

    public StockMovementType Type { get; private set; }

    public int Quantity { get; private set; }

    public string Reason { get; private set; }

    public DateTime Date { get; private set; }

    public StockMovement(StockMovementType type, int quantity, string reason)
    {
        Type = type;
        Quantity = quantity;
        Reason = reason;
        Date = DateTime.UtcNow;
    }
}
