using AlunoContext.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace AlunoContext.Infrastructure.Context;

public class AlunoDbContext : DbContext
{
    public AlunoDbContext(DbContextOptions<AlunoDbContext> options)
        : base(options) { }

    public DbSet<Aluno> Alunos => Set<Aluno>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AlunoDbContext).Assembly);
    }
}
