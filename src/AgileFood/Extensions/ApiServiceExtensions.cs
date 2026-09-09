using AgileFood.Api.Filters;
using AgileFood.Api.Handlers;

namespace AgileFood.Api.Extensions;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers(options => options.Filters.Add<ValidationFilter>());
        services.AddApiDocumentation();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
