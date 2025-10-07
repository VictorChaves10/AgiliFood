namespace AgiliFood.Business.Models;

public class Product
{
    public long Id { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public string? Brand { get; private set; }

    public string Flavor { get; private set; }

    public double Weight { get; private set; }

    public WeightUnitEnum WeightUnit { get; set; }

    public decimal Price { get; private set; }

    public bool IsActive { get; private set; }

    public string? BarCode { get; private set; }

    public string? Image { get; private set; }

    public int ProductCategoryId { get; private set; }

    public ProductCategory? ProductCategory { get; private set; }

    protected Product() { } 

    public Product(string name, string? description, string? brand, string flavor,
                  double weight, decimal price, bool isActive, string? barCode,
                  string? image, int productCategoryId, WeightUnitEnum weightUnit)
    {
        SetName(name);
        SetFlavor(flavor);
        SetWeight(weight, weightUnit);
        ChangePrice(price);

        Description = description;
        Brand = brand;
        IsActive = isActive;
        BarCode = barCode;
        Image = image;
        ProductCategoryId = productCategoryId;
    }


    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(name));

        Name = name;
    }

    public void SetFlavor(string flavor)
    {
        if (string.IsNullOrWhiteSpace(flavor))
            throw new ArgumentException("O sabor é obrigatório.", nameof(flavor));

        Flavor = flavor;
    }

    public void SetWeight(double weight, WeightUnitEnum weightUnit)
    {
        if (weight <= 0)
            throw new ArgumentException("O peso do produto deve ser maior que zero.", nameof(weight));

        WeightUnit = weightUnit;
        Weight = weight;
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Preço não pode ser negativo.", nameof(newPrice));

        Price = newPrice;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void ChangeCategory(int categoryId)
    {
        if (categoryId <= 0)
            throw new ArgumentException("O id da categoria não pode ser nulo.", nameof(categoryId));

        ProductCategoryId = categoryId;
    }

    public void ChangeImage(string? image)
    {
        Image = image;
    }

}
