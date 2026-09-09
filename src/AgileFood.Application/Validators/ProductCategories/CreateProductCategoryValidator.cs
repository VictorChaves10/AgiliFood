using AgileFood.Application.Dtos.ProductCategories;
using FluentValidation;

namespace AgileFood.Application.Validators.ProductCategories;

public sealed class CreateProductCategoryValidator : AbstractValidator<CreateProductCategoryDto>
{
    public CreateProductCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da categoria deve ter no máximo 100 caracteres.");
    }
}
