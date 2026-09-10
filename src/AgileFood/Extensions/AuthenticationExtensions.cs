using System.Text;
using AgileFood.Api.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgileFood.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection("Jwt"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddOptions<TerminalSettings>()
                .Bind(configuration.GetSection("Terminal"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer()
            .AddScheme<AuthenticationSchemeOptions, TerminalApiKeyAuthenticationHandler>(
                TerminalApiKeyDefaults.AuthenticationScheme, null);

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<JwtSettings>>((options, jwtSettings) =>
                {
                    var settings = jwtSettings.Value;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key))
                    };
                });

        // Endpoint sem [Authorize] explicito exige autenticacao por padrao.
        services.AddAuthorizationBuilder()
                .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());

        return services;
    }
}
