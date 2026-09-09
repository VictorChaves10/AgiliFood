namespace AgileFood.Business.Exceptions;

/// <summary>
/// Violacao de unicidade detectada pelo banco. Diferente das checagens de duplicidade
/// feitas nos services, que cobrem o caso comum com mensagem especifica, esta so
/// aparece quando duas requisicoes concorrentes passam pela checagem antes de qualquer
/// uma gravar. A camada de API a traduz para 409.
/// </summary>
public class DuplicateEntryException : Exception
{
    public DuplicateEntryException(string message) : base(message)
    {
    }

    public DuplicateEntryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
