using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Repositories;
using AlunoContext.Infrastructure.Context;

namespace AlunoContext.Infrastructure.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly AlunoDbContext _context;

    public AlunoRepository(AlunoDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Aluno aluno)
    {
        _context.Alunos.Add(aluno);
        _context.SaveChanges();
    }
    public void Excluir(Aluno aluno)
    {
        _context.Alunos.Remove(aluno);
        _context.SaveChanges();
    }
    public Aluno? ObterPorId(Guid id)
    {
        return _context.Alunos.FirstOrDefault(x => x.Id == id);
    }
    public List<Aluno> Listar()
    {
        return _context.Alunos.ToList();
    }
}
