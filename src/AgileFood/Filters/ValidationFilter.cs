using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AgileFood.Api.Filters;

/// <summary>
/// Executa o <see cref="IValidator{T}"/> registrado para cada argumento da action, quando existe,
/// e curto-circuita com 400 antes de chegar no controller. Roda depois do model binding,
/// entao validadores assincronos funcionam normalmente.
/// Argumento sem validator registrado passa direto (ex.: rotas com "long id").
/// </summary>
public sealed class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var services = context.HttpContext.RequestServices;
        var errors = new Dictionary<string, string[]>();

        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            if (services.GetService(validatorType) is not IValidator validator)
                continue;

            var result = await validator.ValidateAsync(
                new ValidationContext<object>(argument),
                context.HttpContext.RequestAborted);

            if (result.IsValid)
                continue;

            foreach (var group in result.Errors.GroupBy(e => e.PropertyName))
                errors[group.Key] = group.Select(e => e.ErrorMessage).ToArray();
        }

        if (errors.Count > 0)
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors)
            {
                Title = "Um ou mais campos sao invalidos.",
                Status = StatusCodes.Status400BadRequest
            });

            return;
        }

        await next();
    }
}
