using CursoContext.Domain.Aggregates;
using CursoContext.Domain.Entities;

namespace CursoContext.Domain.Repositories;

public interface ICursoRepository
{
    Task Adicionar(Curso curso);
    Task AdicionarAula(Aula aula);
    Task Atualizar(Curso curso);
    Task Excluir(Curso curso);
    Task<List<Curso>> Listar();
    Task<Curso> ObterPorId(Guid id);    
}

