using System.ComponentModel.DataAnnotations;

namespace AgileFood.Api.Auth;

public class JwtSettings
{
    [Required(ErrorMessage = "Jwt:Key nao configurado.")]
    [MinLength(32, ErrorMessage = "Jwt:Key deve ter no minimo 32 caracteres (256 bits) para HMAC-SHA256.")]
    public string Key { get; set; } = string.Empty;

    [Required(ErrorMessage = "Jwt:Issuer nao configurado.")]
    public string Issuer { get; set; } = string.Empty;

    [Required(ErrorMessage = "Jwt:Audience nao configurado.")]
    public string Audience { get; set; } = string.Empty;

    [Range(1, 1440, ErrorMessage = "Jwt:ExpirationMinutes deve estar entre 1 e 1440.")]
    public int ExpirationMinutes { get; set; } = 60;
}
