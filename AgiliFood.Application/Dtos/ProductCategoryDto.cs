using System.ComponentModel.DataAnnotations;

namespace AgiliFood.Application.Dtos;

public class ProductCategoryDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Name { get; set; }

    public ICollection<ProductDto>? Products { get; set; } = new List<ProductDto>();
}

