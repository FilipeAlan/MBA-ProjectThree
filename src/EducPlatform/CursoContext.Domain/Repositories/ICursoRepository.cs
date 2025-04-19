using CursoContext.Domain.Aggregates;

namespace CursoContext.Domain.Repositories;

public interface ICursoRepository
{
    void Adicionar(Curso aluno);
    void Excluir(Curso aluno);
    List<Curso> Listar();
    Curso ObterPorId(Guid id);    
}

