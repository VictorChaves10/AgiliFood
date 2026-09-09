using AgileFood.Application.Dtos.Users;
using AgileFood.Business.Models.Users;
using FluentValidation;

namespace AgileFood.Application.Validators.Users;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(cpf => User.IsValidCpf(User.NormalizeCpf(cpf)))
            .WithMessage("O CPF informado não é válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");

        RuleFor(x => x.TransactionPin)
            .NotEmpty().WithMessage("O PIN é obrigatório.")
            .Matches("^\\d{4}$").WithMessage("O PIN deve ter exatamente 4 digitos.");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("O perfil informado não é válido.");
    }
}
