using CursoContext.Application.Queries.ListarCurso;
using CursoContext.Domain.Aggregates;
using CursoContext.Tests.Unit.Fakes;

namespace CursoContext.Tests.Unit.Cursos;

public class ListarCursosTests
{
    private readonly CursoRepositorioFake _repositorio;
    private readonly ListarCursosHandler _handler;

    public ListarCursosTests()
    {
        _repositorio = new CursoRepositorioFake();
        _handler = new ListarCursosHandler(_repositorio);
    }

    [Fact(DisplayName = "Deve retornar todos os cursos cadastrados")]
    public async Task DeveListarTodosCursos()
    {
        // Arrange
        var curso1 = new Curso("Curso 1", new("Descrição 1", "Objetivos 1"), "Teste");
        var curso2 = new Curso("Curso 2", new("Descrição 2", "Objetivos 2"), "Teste");
        await _repositorio.Adicionar(curso1);
        await _repositorio.Adicionar(curso2);

        // Act
        var resultado = await _handler.Handle(new ListarCursosQuery(),CancellationToken.None);

        // Assert
        Assert.Equal(2, resultado.Count());
        Assert.Contains(resultado, c => c.Nome == "Curso 1");
        Assert.Contains(resultado, c => c.Nome == "Curso 2");
    }
}
