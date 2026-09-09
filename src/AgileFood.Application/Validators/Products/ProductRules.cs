using FluentValidation;

namespace AgileFood.Application.Validators.Products;

/// <summary>
/// Regras dos campos de produto, compartilhadas entre criacao e edicao.
/// </summary>
internal static class ProductRules
{
    public static IRuleBuilderOptions<T, string> ProductName<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do produto deve ter no máximo 100 caracteres.");

    public static IRuleBuilderOptions<T, string?> ProductDescription<T>(this IRuleBuilder<T, string?> rule) =>
        rule.MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

    public static IRuleBuilderOptions<T, string?> ProductBrand<T>(this IRuleBuilder<T, string?> rule) =>
        rule.MaximumLength(100).WithMessage("A marca deve ter no máximo 100 caracteres.");

    public static IRuleBuilderOptions<T, string?> ProductFlavor<T>(this IRuleBuilder<T, string?> rule) =>
        rule.NotEmpty().WithMessage("O sabor do produto é obrigatório.")
            .MaximumLength(100).WithMessage("O sabor deve ter no máximo 100 caracteres.");

    public static IRuleBuilderOptions<T, string?> ProductBarCode<T>(this IRuleBuilder<T, string?> rule) =>
        rule.MaximumLength(50).WithMessage("O código de barras deve ter no máximo 50 caracteres.");

    public static IRuleBuilderOptions<T, string?> ProductImage<T>(this IRuleBuilder<T, string?> rule) =>
        rule.MaximumLength(200).WithMessage("O caminho da imagem deve ter no máximo 200 caracteres.");

    public static IRuleBuilderOptions<T, decimal> ProductPrice<T>(this IRuleBuilder<T, decimal> rule) =>
        rule.GreaterThan(0).WithMessage("O preço deve ser maior que zero.")
            .PrecisionScale(18, 2, ignoreTrailingZeros: true)
            .WithMessage("O preço deve ter no máximo 2 casas decimais.");

    public static IRuleBuilderOptions<T, decimal> ProductWeightAmount<T>(this IRuleBuilder<T, decimal> rule) =>
        rule.GreaterThan(0).WithMessage("O peso deve ser maior que zero.")
            .PrecisionScale(10, 3, ignoreTrailingZeros: true)
            .WithMessage("O peso deve ter no máximo 3 casas decimais.");

    public static IRuleBuilderOptions<T, int> ProductCategoryId<T>(this IRuleBuilder<T, int> rule) =>
        rule.GreaterThan(0).WithMessage("A categoria do produto é obrigatória.");
}
