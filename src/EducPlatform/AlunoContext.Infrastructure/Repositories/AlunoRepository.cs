using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Repositories;
using AlunoContext.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AlunoContext.Infrastructure.Repositories;
public class AlunoRepository : IAlunoRepository
{
    private readonly AlunoDbContext _context;

    public AlunoRepository(AlunoDbContext context)
    {
        _context = context;
    }
    

    public async Task Adicionar(Aluno aluno)
    {
        await _context.Alunos.AddAsync(aluno);
    }

    public Task Atualizar(Aluno aluno)
    {
        _context.Alunos.Update(aluno);
        return Task.CompletedTask;
    }

    public Task Excluir(Aluno aluno)
    {
        _context.Alunos.Remove(aluno);
        return Task.CompletedTask;
    }

    public async Task<Aluno?> ObterPorId(Guid id)
    {
        return await _context.Alunos
            .Include(a => a.Matriculas)
            .Include(a => a.Certificados)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Aluno>> Listar()
    {
        return await _context.Alunos
            .Include(a => a.Matriculas)
            .Include(a => a.Certificados)
            .ToListAsync();
    }
    public async Task<Aluno?> ObterAlunoPorMatriculaId(Guid matriculaId)
    {
        return await _context.Alunos
            .Include(a => a.Matriculas)
            .FirstOrDefaultAsync(a => a.Matriculas.Any(m => m.Id == matriculaId));
    }
}
