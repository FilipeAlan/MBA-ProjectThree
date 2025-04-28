using Microsoft.EntityFrameworkCore;
using PagamentoContext.Domain.Aggregates;

namespace PagamentoContext.Infrastructure.Context;

public class PagamentoDbContext : DbContext
{
    public PagamentoDbContext(DbContextOptions<PagamentoDbContext> options) : base(options) { }

    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoDbContext).Assembly);
    }
}