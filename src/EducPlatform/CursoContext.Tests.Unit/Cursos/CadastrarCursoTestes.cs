using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Tests.Shared.Fakes;
using CursoContext.Tests.Unit.Fakes;
using Xunit;

namespace CursoContext.Tests.Unit.Cursos;

public class CadastrarCursoTests
{
    private readonly CursoRepositorioFake _repositorioFake;
    private readonly UsuarioContextoFake _usuarioFake;
    private readonly UnitOfWorkFake _unitOfWorkFake;
    private readonly CadastrarCursoHandler _handler;

    public CadastrarCursoTests()
    {
        _repositorioFake = new CursoRepositorioFake();
        _usuarioFake = new UsuarioContextoFake();
        _unitOfWorkFake = new UnitOfWorkFake();
        _handler = new CadastrarCursoHandler(_repositorioFake, _usuarioFake, _unitOfWorkFake);
    }

    [Fact(DisplayName = "Deve cadastrar curso com dados válidos")]
    public async Task DeveCadastrarCurso_Valido()
    {
        // Arrange
        var comando = new CadastrarCursoComando("Curso de TDD", "Aprenda a testar antes de codar");

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Single(_repositorioFake.Cursos);
        Assert.Equal("Curso de TDD", _repositorioFake.Cursos[0].Nome);
    }

    [Fact(DisplayName = "Não deve cadastrar curso com nome vazio")]
    public async Task NaoDeveCadastrarCurso_ComNomeVazio()
    {
        // Arrange
        var comando = new CadastrarCursoComando("", "Descrição válida");

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("nome", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve cadastrar curso com descrição vazia")]
    public async Task NaoDeveCadastrarCurso_ComDescricaoVazia()
    {
        // Arrange
        var comando = new CadastrarCursoComando("Curso válido", "");

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("descrição", resultado.Mensagem.ToLower());
    }
}
