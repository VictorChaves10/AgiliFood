using AgileFood.Application.Dtos.Stock;
using FluentValidation;

namespace AgileFood.Application.Validators.Stock;

public sealed class CreateStockItemValidator : AbstractValidator<CreateStockItemDto>
{
    public CreateStockItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("O produto é obrigatório.");

        RuleFor(x => x.InitialQuantity)
            .GreaterThan(0).WithMessage("A quantidade inicial deve ser maior que zero.");
    }
}
