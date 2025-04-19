using CursoContext.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace CursoContext.Infrastructure.Context;
    public class CursoDbContext : DbContext
    {
        public CursoDbContext(DbContextOptions<CursoDbContext> options) : base(options) { }

        public DbSet<Curso> Cursos => Set<Curso>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CursoDbContext).Assembly);
        }
    }

