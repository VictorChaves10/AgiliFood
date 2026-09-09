using AgileFood.Application.Dtos.Consumptions;
using AgileFood.Application.Validators.Shared;
using FluentValidation;

namespace AgileFood.Application.Validators.Consumptions;

public sealed class RegisterConsumptionValidator : AbstractValidator<RegisterConsumptionDto>
{
    public RegisterConsumptionValidator()
    {
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("O código do funcionário é obrigatório.")
            .MaximumLength(20).WithMessage("O código do funcionário deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Pin).ValidTransactionPin();

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("O carrinho está vazio.");

        RuleForEach(x => x.Items)
            .SetValidator(new RegisterConsumptionItemValidator());
    }
}
