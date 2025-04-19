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
       await _context.SaveChangesAsync();
    }
    public async Task Atualizar(Aluno aluno)
    {
        // Se aluno foi recuperado pelo contexto, isso pode ser omitido:
        _context.Alunos.Update(aluno);
       await _context.SaveChangesAsync(); // aplica alterações no banco
    }
    public async Task Excluir(Aluno aluno)
    {
        _context.Alunos.Remove(aluno);
        await _context.SaveChangesAsync();
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
}
