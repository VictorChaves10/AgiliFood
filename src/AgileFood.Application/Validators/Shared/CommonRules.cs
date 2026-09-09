using AgileFood.Business.Models.Users;
using FluentValidation;

namespace AgileFood.Application.Validators.Shared;

/// <summary>
/// Regras compartilhadas entre validators. Nenhuma delas reimplementa uma regra
/// que ja existe no dominio: quando existe, ela e invocada (ex.: <see cref="User.IsValidCpf"/>).
/// </summary>
public static class CommonRules
{
    public static IRuleBuilderOptions<T, string> ValidPersonName<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(150).WithMessage("O nome deve ter no máximo 150 caracteres.");

    public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail informado não é válido.")
            .MaximumLength(200).WithMessage("O e-mail deve ter no máximo 200 caracteres.");

    public static IRuleBuilderOptions<T, string> ValidCpf<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(cpf => User.IsValidCpf(User.NormalizeCpf(cpf)))
            .WithMessage("O CPF informado não é válido.");

    public static IRuleBuilderOptions<T, string> ValidPassword<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");

    public static IRuleBuilderOptions<T, string> ValidTransactionPin<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("O PIN é obrigatório.")
            .Must(pin => TransactionPin.IsValid(pin))
            .WithMessage($"O PIN deve ter exatamente {TransactionPin.Length} dígitos.");
}
