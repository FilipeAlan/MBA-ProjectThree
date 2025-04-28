using AlunoContext.Domain.Aggregates;

namespace AlunoContext.Domain.Repositories;

public interface IAlunoRepository
{    
    Task Adicionar(Aluno aluno);
    Task Excluir(Aluno aluno);
    Task Atualizar(Aluno aluno);
    Task<List<Aluno>> Listar();
    Task<Aluno> ObterPorId(Guid id);
}
