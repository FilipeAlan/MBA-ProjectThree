namespace PagamentoContext.Domain.ValueObjects;
public class DadosCartao
{
    public string Numero { get; private set; }
    public string NomeTitular { get; private set; }
    public string Validade { get; private set; }
    public string CVV { get; private set; }

    protected DadosCartao() { } // EF Core

    public DadosCartao(string numero, string nomeTitular, string validade, string cvv)
    {
        Numero = numero ?? throw new ArgumentNullException(nameof(numero));
        NomeTitular = nomeTitular ?? throw new ArgumentNullException(nameof(nomeTitular));
        Validade = validade ?? throw new ArgumentNullException(nameof(validade));
        CVV = cvv ?? throw new ArgumentNullException(nameof(cvv));
    }
}
