using CursoContext.Domain.Aggregates;

namespace CursoContext.Domain.Repositories;

public interface ICursoRepository
{
    Task Adicionar(Curso curso);
    Task Atualizar(Curso curso);
    Task Excluir(Curso curso);
    Task<List<Curso>> Listar();
    Task<Curso> ObterPorId(Guid id);    
}

