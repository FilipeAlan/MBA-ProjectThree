using PagamentoContext.Domain.Enums;
using PagamentoContext.Domain.ValueObjects;

namespace PagamentoContext.Domain.Aggregates;

public class Pagamento
{
    public Guid Id { get; private set; }
    public Guid MatriculaId { get; private set; }
    public decimal Valor { get; private set; }
    public StatusPagamento Status { get; private set; }
    public DadosCartao DadosCartao { get; private set; }
    public DateTime DataPagamento { get; private set; }

    protected Pagamento() { } // EF Core

    public Pagamento(Guid matriculaId, decimal valor, DadosCartao dadosCartao)
    {
        Id = Guid.NewGuid();
        MatriculaId = matriculaId;
        Valor = valor;
        DadosCartao = dadosCartao;
        Status = StatusPagamento.Aguardando;
        DataPagamento = DateTime.UtcNow;
    }

    public void ConfirmarPagamento()
    {
        Status = StatusPagamento.Pago;
    }

    public void RejeitarPagamento()
    {
        Status = StatusPagamento.Rejeitado;
    }
}
