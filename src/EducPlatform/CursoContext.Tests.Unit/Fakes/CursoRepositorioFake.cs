using CursoContext.Domain.Aggregates;
using CursoContext.Domain.Repositories;

namespace CursoContext.Tests.Unit.Fakes;
public class CursoRepositorioFake:ICursoRepository
{
    public List<Curso> Cursos { get; } = new();
    public void Adicionar(Curso curso) => Cursos.Add(curso);
    public void Excluir(Curso curso) => Cursos.Remove(curso);
    public Curso? ObterPorId(Guid id) => Cursos.Find(c => c.Id == id);
    public List<Curso> Listar() => Cursos.ToList();
}

