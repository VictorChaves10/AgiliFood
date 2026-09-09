using AgileFood.Application.Dtos.Catalogs;
using FluentValidation;

namespace AgileFood.Application.Validators.Catalogs;

public sealed class CreateCatalogItemValidator : AbstractValidator<CreateCatalogItemDto>
{
    public CreateCatalogItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("O produto é obrigatório.");
    }
}
