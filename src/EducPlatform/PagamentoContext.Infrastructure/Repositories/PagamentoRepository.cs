using Microsoft.EntityFrameworkCore;
using PagamentoContext.Domain.Aggregates;
using PagamentoContext.Domain.Repositories;
using PagamentoContext.Infrastructure.Context;

namespace PagamentoContext.Infrastructure.Repositories;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly PagamentoDbContext _context;

    public PagamentoRepository(PagamentoDbContext context)
    {
        _context = context;
    }

    public async Task Adicionar(Pagamento pagamento)
    {
        await _context.Pagamentos.AddAsync(pagamento);
    }

    public async Task<Pagamento> ObterPorId(Guid id)
    {
        return await _context.Pagamentos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public Task Atualizar(Pagamento pagamento)
    {
        _context.Pagamentos.Update(pagamento);
        return Task.CompletedTask;
    }
}
