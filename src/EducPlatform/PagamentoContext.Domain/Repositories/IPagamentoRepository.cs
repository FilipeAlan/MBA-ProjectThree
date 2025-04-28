namespace PagamentoContext.Domain.Repositories;

using PagamentoContext.Domain.Aggregates;

public interface IPagamentoRepository
{
    Task Adicionar(Pagamento pagamento);
    Task<Pagamento> ObterPorId(Guid id);
    Task Atualizar(Pagamento pagamento);
}