using PagamentoContext.Domain.Aggregates;
using PagamentoContext.Domain.Repositories;

namespace PagamentoContext.Tests.Unit.Fakes;

public class PagamentoRepositoryFake : IPagamentoRepository
{
    public List<Pagamento> Pagamentos { get; } = new();

    public Task Adicionar(Pagamento pagamento)
    {
        Pagamentos.Add(pagamento);
        return Task.CompletedTask;
    }

    public Task Atualizar(Pagamento pagamento)
    {
        throw new NotImplementedException();
    }

    public Task<Pagamento> ObterPorId(Guid id)
    {
        throw new NotImplementedException();
    }
}
