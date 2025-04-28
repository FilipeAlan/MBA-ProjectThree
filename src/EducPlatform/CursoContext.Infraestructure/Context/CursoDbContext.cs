using CursoContext.Domain.Aggregates;
using CursoContext.Domain.Entities;
using CursoContext.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoContext.Infrastructure.Context;
    public class CursoDbContext : DbContext
    {
        public CursoDbContext(DbContextOptions<CursoDbContext> options) : base(options) { }

        public DbSet<Curso> Cursos => Set<Curso>();
        public DbSet<Aula> Aulas => Set<Aula>();      

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CursoDbContext).Assembly);
        }
    }

