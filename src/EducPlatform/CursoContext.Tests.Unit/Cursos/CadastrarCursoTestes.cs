using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Handlers;
using CursoContext.Tests.Unit.Fakes;
using Xunit;

namespace CursoContext.Tests.Unit.Cursos;

public class CadastrarCursoTests
{
    [Fact(DisplayName = "Deve cadastrar curso com dados válidos")]
    public void DeveCadastrarCurso_Valido()
    {
        // Arrange
        var comando = new CadastrarCursoComando("Curso de TDD", "Aprenda a testar antes de codar");
        var repositorio = new CursoRepositorioFake();
        var handler = new CadastrarCursoHandler(repositorio);

        // Act
        var resultado = handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Single(repositorio.Cursos);
        Assert.Equal("Curso de TDD", repositorio.Cursos[0].Nome);
    }

    [Fact(DisplayName = "Não deve cadastrar curso com nome vazio")]
    public void NaoDeveCadastrarCurso_ComNomeVazio()
    {
        var comando = new CadastrarCursoComando("", "Descrição válida");
        var repositorio = new CursoRepositorioFake();
        var handler = new CadastrarCursoHandler(repositorio);

        var resultado = handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("nome", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve cadastrar curso com descrição vazia")]
    public void NaoDeveCadastrarCurso_ComDescricaoVazia()
    {
        var comando = new CadastrarCursoComando("Curso válido", "");
        var repositorio = new CursoRepositorioFake();
        var handler = new CadastrarCursoHandler(repositorio);

        var resultado = handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("descrição", resultado.Mensagem.ToLower());
    }
}

