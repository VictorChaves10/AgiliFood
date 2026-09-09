using AgileFood.Application.Dtos.Auth;
using AgileFood.Application.Validators.Shared;
using FluentValidation;

namespace AgileFood.Application.Validators.Auth;

public sealed class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).ValidEmail();

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.");
    }
}
