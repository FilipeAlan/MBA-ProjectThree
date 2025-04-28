using CursoContext.Application.Queries.ObterCurso;
using CursoContext.Domain.Aggregates;
using CursoContext.Tests.Unit.Fakes;

namespace CursoContext.Tests.Unit.Cursos;

public class ObterCursoTests
{
    private readonly CursoRepositorioFake _repositorio;
    private readonly ObterCursoPorIdHandler _handler;

    public ObterCursoTests()
    {
        _repositorio = new CursoRepositorioFake();
        _handler = new ObterCursoPorIdHandler(_repositorio);
    }

    [Fact(DisplayName = "Deve retornar curso quando o ID existir")]
    public async Task DeveRetornarCurso_QuandoIdExistir()
    {
        // Arrange
        var curso = new Curso("Curso X", new("Descrição X", "Objetivos X"), "Teste");
        await _repositorio.Adicionar(curso);

        // Act
        var resultado = await _handler.Handle(new ObterCursoPorIdQuery(curso.Id));

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(curso.Id, resultado!.Id);
        Assert.Equal("Curso X", resultado.Nome);
        Assert.Equal("Descrição X", resultado.Descricao);
    }

    [Fact(DisplayName = "Deve retornar nulo quando o ID não existir")]
    public async Task DeveRetornarNulo_QuandoIdNaoExistir()
    {
        // Act
        var resultado = await _handler.Handle(new ObterCursoPorIdQuery(Guid.NewGuid()));

        // Assert
        Assert.Null(resultado);
    }
}
