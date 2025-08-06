namespace AgiliFood.Application.Dtos;

public class ProductCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
}

