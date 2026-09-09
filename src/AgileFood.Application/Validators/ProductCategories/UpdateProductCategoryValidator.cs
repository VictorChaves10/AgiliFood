using AgileFood.Application.Dtos.ProductCategories;
using FluentValidation;

namespace AgileFood.Application.Validators.ProductCategories;

public sealed class UpdateProductCategoryValidator : AbstractValidator<UpdateProductCategoryDto>
{
    public UpdateProductCategoryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O identificador da categoria é obrigatório.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da categoria deve ter no máximo 100 caracteres.");
    }
}
