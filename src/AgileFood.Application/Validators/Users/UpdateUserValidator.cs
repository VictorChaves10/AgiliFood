using AgileFood.Application.Dtos.Users;
using AgileFood.Application.Validators.Shared;
using FluentValidation;

namespace AgileFood.Application.Validators.Users;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O identificador do usuário é obrigatório.");

        RuleFor(x => x.Name).ValidPersonName();
        RuleFor(x => x.Email).ValidEmail();
        RuleFor(x => x.Cpf).ValidCpf();

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("O perfil informado não é válido.");
    }
}
