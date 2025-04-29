using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Repositories;

namespace PagamentoContext.Tests.Unit.Fakes;

public class AlunoRepositoryPagamentoFake : IAlunoRepository
{
    private readonly List<Aluno> _alunos = new();

    public void AdicionarAluno(Aluno aluno)
    {
        _alunos.Add(aluno);
    }

    public Task<Aluno?> ObterAlunoPorMatriculaId(Guid matriculaId)
    {
        var aluno = _alunos.FirstOrDefault(a => a.Matriculas.Any(m => m.Id == matriculaId));
        return Task.FromResult(aluno);
    }

    public Task Atualizar(Aluno aluno)
    {
        // Para o fake de pagamento, não precisa fazer nada aqui.
        return Task.CompletedTask;
    }

    // Métodos que não são usados nos testes de pagamento podem lançar exceção
    public Task<Aluno> ObterPorId(Guid id) => throw new NotImplementedException();
    public Task<List<Aluno>> Listar() => throw new NotImplementedException();
    public Task Adicionar(Aluno aluno) => throw new NotImplementedException();
    public Task Excluir(Aluno aluno) => throw new NotImplementedException();
}
