using AgileFood.Application.Dtos.Products;
using FluentValidation;

namespace AgileFood.Application.Validators.Products;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O identificador do produto é obrigatório.");

        RuleFor(x => x.Name).ProductName();
        RuleFor(x => x.Description).ProductDescription();
        RuleFor(x => x.Brand).ProductBrand();
        RuleFor(x => x.Flavor).ProductFlavor();
        RuleFor(x => x.BarCode).ProductBarCode();
        RuleFor(x => x.Image).ProductImage();
        RuleFor(x => x.Price).ProductPrice();
        RuleFor(x => x.WeightAmount).ProductWeightAmount();
        RuleFor(x => x.ProductCategoryId).ProductCategoryId();

        RuleFor(x => x.WeightUnit)
            .IsInEnum().WithMessage("A unidade de peso informada não é válida.");
    }
}
