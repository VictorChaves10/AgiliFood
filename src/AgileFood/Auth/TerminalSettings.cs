using System.ComponentModel.DataAnnotations;

namespace AgileFood.Api.Auth;

public class TerminalSettings
{
    /// <summary>
    /// Chave compartilhada com os terminais de consumo.
    /// </summary>
    [Required(ErrorMessage = "Terminal:ApiKey nao configurado.")]
    [MinLength(32, ErrorMessage = "Terminal:ApiKey deve ter no minimo 32 caracteres.")]
    public string ApiKey { get; set; } = string.Empty;
}
