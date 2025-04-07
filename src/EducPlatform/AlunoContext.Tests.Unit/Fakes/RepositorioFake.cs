using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Repositories;

namespace AlunoContext.Tests.Unit.Fakes;

public class RepositorioFake : IAlunoRepository
{
    public List<Aluno> Alunos { get; } = new();

    public void Adicionar(Aluno aluno) => Alunos.Add(aluno);
    public void Excluir(Aluno aluno) => Alunos.Remove(aluno);
    public Aluno? ObterPorId(Guid id) => Alunos.Find(a => a.Id == id);    
    public List<Aluno> Listar() => Alunos.ToList();
}
