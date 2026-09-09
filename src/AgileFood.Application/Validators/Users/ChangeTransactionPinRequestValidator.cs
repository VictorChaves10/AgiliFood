using AgileFood.Application.Dtos.Users;
using AgileFood.Application.Validators.Shared;
using FluentValidation;

namespace AgileFood.Application.Validators.Users;

public sealed class ChangeTransactionPinRequestValidator : AbstractValidator<ChangeTransactionPinRequestDto>
{
    public ChangeTransactionPinRequestValidator()
    {
        RuleFor(x => x.CurrentPin)
            .NotEmpty().WithMessage("O PIN atual é obrigatório.");

        RuleFor(x => x.NewPin)
            .ValidTransactionPin()
            .NotEqual(x => x.CurrentPin)
            .WithMessage("O novo PIN deve ser diferente do atual.");
    }
}
