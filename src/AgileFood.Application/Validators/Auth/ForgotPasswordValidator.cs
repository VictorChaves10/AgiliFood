using AgileFood.Application.Dtos.Auth;
using AgileFood.Application.Validators.Shared;
using FluentValidation;

namespace AgileFood.Application.Validators.Auth;

public sealed class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email).ValidEmail();
    }
}
