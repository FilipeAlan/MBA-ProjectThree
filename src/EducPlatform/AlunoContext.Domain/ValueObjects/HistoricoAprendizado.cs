namespace AlunoContext.Domain.ValueObjects;

public class HistoricoAprendizado
{
    public DateTime UltimaAtualizacao { get; private set; }
    public double PercentualConcluido { get; private set; }

    protected HistoricoAprendizado() { }

    public HistoricoAprendizado(double percentualConcluido)
    {
        UltimaAtualizacao = DateTime.UtcNow;
        PercentualConcluido = percentualConcluido;
    }

    public HistoricoAprendizado Avancar(double novoPercentual)
    {
        if (novoPercentual < PercentualConcluido)
            throw new InvalidOperationException("Não é permitido regredir o progresso.");

        return new HistoricoAprendizado(novoPercentual);
    }
}
