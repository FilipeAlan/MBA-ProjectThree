using CursoContext.Domain.Aggregates;
using CursoContext.Domain.Entities;
using CursoContext.Domain.Repositories;

namespace CursoContext.Tests.Unit.Fakes;

public class CursoRepositorioFake : ICursoRepository
{
    public List<Curso> Cursos { get; } = new();

    public Task Adicionar(Curso curso)
    {
        Cursos.Add(curso);
        return Task.CompletedTask;
    }

    public Task Excluir(Curso curso)
    {
        Cursos.Remove(curso);
        return Task.CompletedTask;
    }

    public Task<Curso?> ObterPorId(Guid id)
    {
        var curso = Cursos.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(curso);
    }

    public Task<List<Curso>> Listar()
    {
        return Task.FromResult(Cursos.ToList());
    }

    public Task Atualizar(Curso curso)
    {
        var index = Cursos.FindIndex(x => x.Id == curso.Id);
        if (index != -1)
            Cursos[index] = curso;

        return Task.CompletedTask;
    }
    public Task AdicionarAula(Aula aula)
    {        
        return Task.CompletedTask;
    }

}
