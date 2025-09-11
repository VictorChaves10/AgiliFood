using AgiliFood.Business.Models;
using System.ComponentModel.DataAnnotations;

namespace AgiliFood.Application.Dtos;

public class ProductDto
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Name { get; set; }

    [StringLength(300, ErrorMessage = "A descrição deve ter no máximo 300 caracteres.")]
    public string? Description { get; set; }

    public string? Brand { get; set; }

    [Required(ErrorMessage = "O sabor é obrigatório.")]
    public string Flavor { get; set; }

    [Required(ErrorMessage = "O peso é obrigatório.")]
    public double Weight { get; set; }

    [Required(ErrorMessage = "A unidade de peso é obrigatória.")]
    public WeightUnitEnum WeightUnit { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "O código de barras é obrigatório.")]
    [StringLength(50, ErrorMessage = "O código de barras deve ter no máximo 50 caracteres.")]
    public string? BarCode { get; set; }

    [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
    public int ProductCategoryId { get; set; }

    public string CategoryName { get; set; }

    public string? Image { get; set; }

    public string ImageUpload { get; set; }
}
