namespace AgiliFood.Business.Models;

public class ProductCategory
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Product>? Products { get; set; }

    protected ProductCategory() { }

    public ProductCategory(string name)
    {
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}