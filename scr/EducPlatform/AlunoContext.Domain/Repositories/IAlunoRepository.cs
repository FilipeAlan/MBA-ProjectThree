using AlunoContext.Domain.Aggregates;

namespace AlunoContext.Domain.Repositories;

public interface IAlunoRepository
{
    void Adicionar(Aluno aluno);
    void Excluir(Aluno aluno);
    List<Aluno> Listar();
    Aluno ObterPorId(Guid id);
}
