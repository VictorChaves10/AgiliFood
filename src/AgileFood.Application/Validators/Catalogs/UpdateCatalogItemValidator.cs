using AgileFood.Application.Dtos.Catalogs;
using FluentValidation;

namespace AgileFood.Application.Validators.Catalogs;

public sealed class UpdateCatalogItemValidator : AbstractValidator<UpdateCatalogItemDto>
{
    public UpdateCatalogItemValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O identificador do item do catálogo é obrigatório.");
    }
}
