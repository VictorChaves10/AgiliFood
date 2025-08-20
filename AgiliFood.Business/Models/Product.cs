namespace AgiliFood.Business.Models;

public class Product
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Brand { get; set; }

    public string Flavor { get; set; }

    public Wight Weight { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public string BarCode { get; set; }
    
    public string Image { get; set; }

    public int ProductCategoryId { get; set; }

    public ProductCategory ProductCategory { get; set; }

}
