using AgileFood.Application.Dtos.Auth;
using AgileFood.Application.Validators.Shared;
using FluentValidation;

namespace AgileFood.Application.Validators.Auth;

public sealed class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator()
    {
        RuleFor(x => x.Email).ValidEmail();

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("O token de redefinição é obrigatório.");

        RuleFor(x => x.NewPassword).ValidPassword();
    }
}
