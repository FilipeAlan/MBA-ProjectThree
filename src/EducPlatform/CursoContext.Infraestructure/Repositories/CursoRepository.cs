using CursoContext.Domain.Aggregates;
using CursoContext.Domain.Entities;
using CursoContext.Domain.Repositories;
using CursoContext.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CursoContext.Infrastructure.Repositories;

public class CursoRepository : ICursoRepository
{
    private readonly CursoDbContext _context;

    public CursoRepository(CursoDbContext context)
    {
        _context = context;
    }
    public Task AdicionarAula(Aula aula)
    {
        _context.Aulas.Add(aula);
        return Task.CompletedTask;
    }
    public async Task Adicionar(Curso curso)
    {
        await _context.Cursos.AddAsync(curso);
    }

    public Task Atualizar(Curso curso)
    {
        _context.Entry(curso).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task Excluir(Curso curso)
    {
        _context.Cursos.Remove(curso);
        return Task.CompletedTask;
    }

    public async Task<Curso?> ObterPorId(Guid id)
    {
        var curso = await _context.Cursos
            .Include(c => c.Aulas)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (curso != null)
        {
            _context.Attach(curso);
        }

        return curso;
    }

    public async Task<List<Curso>> Listar()
    {
        return await _context.Cursos
            .Include(c => c.Aulas)
            .ToListAsync();
    }
}
