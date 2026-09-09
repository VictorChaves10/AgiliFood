using AgileFood.Application.Dtos.Consumptions;
using FluentValidation;

namespace AgileFood.Application.Validators.Consumptions;

public sealed class RegisterConsumptionItemValidator : AbstractValidator<RegisterConsumptionItemDto>
{
    public RegisterConsumptionItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0).WithMessage("O produto do item é obrigatório.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("A quantidade do item deve ser maior que zero.");
    }
}
