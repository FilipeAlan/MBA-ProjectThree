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
    public async Task AdicionarAula(Aula aula)
    {
        await _context.Aulas.AddAsync(aula);        
    }
    public async Task Adicionar(Curso curso)
    {
        await _context.Cursos.AddAsync(curso);
    }
    public Task Atualizar(Curso curso)
    {        
        _context.Cursos.Update(curso);
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

        return curso;
    }

    public async Task<List<Curso>> Listar()
    {        

        return await _context.Cursos
            .Include(c => c.Aulas)
            .AsNoTracking()
            .ToListAsync();
    }
}
