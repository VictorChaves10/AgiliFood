using AgileFood.Business.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AgileFood.Api.Handlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = Map(exception);
        var isServerError = statusCode == StatusCodes.Status500InternalServerError;

        if (isServerError)
            _logger.LogError(exception, "Erro nao tratado em {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);

        httpContext.Response.StatusCode = statusCode;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = isServerError
                    ? "Ocorreu um erro inesperado. Tente novamente."
                    : exception.Message
            }
        });
    }

    private static (int StatusCode, string Title) Map(Exception exception) => exception switch
    {
        AccountLockedException => (StatusCodes.Status429TooManyRequests, "Muitas tentativas."),

        ConcurrencyConflictException => (StatusCodes.Status409Conflict, "Conflito de concorrencia."),

        DomainException => (StatusCodes.Status400BadRequest, "Requisicao invalida."),

        // Todo o resto e bug: ArgumentException (contrato interno violado),
        // InvalidOperationException do BCL/EF, falhas de infra. 500 + log, sem vazar mensagem.
        _ => (StatusCodes.Status500InternalServerError, "Erro interno.")
    };
}
