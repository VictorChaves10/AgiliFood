using AgileFood.Application.Dtos.Stock;
using FluentValidation;

namespace AgileFood.Application.Validators.Stock;

public sealed class RegisterStockMovementValidator : AbstractValidator<RegisterStockMovementDto>
{
    public RegisterStockMovementValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

        RuleFor(x => x.Reason)
            .MaximumLength(300).WithMessage("O motivo deve ter no máximo 300 caracteres.");
    }
}
