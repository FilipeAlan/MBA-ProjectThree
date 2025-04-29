using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Repositories;

namespace AlunoContext.Tests.Unit.Fakes;

public class AlunoRepositorioFake : IAlunoRepository
{
    public List<Aluno> Alunos { get; } = new();

    public Task Adicionar(Aluno aluno)
    {
        Alunos.Add(aluno);
        return Task.CompletedTask;
    }

    public Task Atualizar(Aluno aluno)
    {
        var index = Alunos.FindIndex(x => x.Id == aluno.Id);
        if (index != -1)
            Alunos[index] = aluno;

        return Task.CompletedTask;
    }

    public Task Excluir(Aluno aluno)
    {
        Alunos.Remove(aluno);
        return Task.CompletedTask;
    }

    public Task<Aluno?> ObterPorId(Guid id)
    {
        var aluno = Alunos.Find(a => a.Id == id);
        return Task.FromResult<Aluno?>(aluno);
    }

    public Task<List<Aluno>> Listar()
    {
        return Task.FromResult(Alunos.ToList());
    }

    public Task<Aluno?> ObterAlunoPorMatriculaId(Guid matriculaId)
    {
        var aluno = Alunos.FirstOrDefault(a => a.Matriculas.Any(m => m.Id == matriculaId));
        return Task.FromResult(aluno);
    }
}
