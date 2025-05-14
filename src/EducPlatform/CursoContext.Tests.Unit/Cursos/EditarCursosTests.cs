using CursoContext.Application.Commands.EditarCurso;
using CursoContext.Domain.Aggregates;
using CursoContext.Domain.ValueObjects;
using CursoContext.Tests.Shared.Fakes;
using CursoContext.Tests.Unit.Fakes;

namespace CursoContext.Tests.Unit.Cursos;

public class EditarCursoTests
{
    private readonly CursoRepositorioFake _repositorio;
    private readonly UsuarioContextoFake _usuario;
    private readonly UnitOfWorkFake _unitOfWork;
    private readonly EditarCursoHandler _handler;

    public EditarCursoTests()
    {
        _repositorio = new CursoRepositorioFake();
        _usuario = new UsuarioContextoFake();
        _unitOfWork = new UnitOfWorkFake();
        _handler = new EditarCursoHandler(_repositorio, _usuario, _unitOfWork);
    }

    [Fact(DisplayName = "Deve editar curso quando os dados forem válidos")]
    public async Task DeveEditarCurso_Valido()
    {
        // Arrange
        var curso = new Curso("Curso Antigo", new ConteudoProgramatico("Desc antiga", "Obj"), "Teste");
        await _repositorio.Adicionar(curso);

        var comando = new EditarCursoComando(curso.Id, "Curso Novo", "Nova descrição");

        // Act
        var resultado = await _handler.Handle(comando, CancellationToken.None);

        // Assert
        Assert.True(resultado.Sucesso);
        var atualizado = await _repositorio.ObterPorId(curso.Id);

        Assert.NotNull(atualizado);
        Assert.Equal("Curso Novo", atualizado!.Nome);
        Assert.Equal("Nova descrição", atualizado.Conteudo.Descricao);
    }

    [Fact(DisplayName = "Não deve editar curso inexistente")]
    public async Task NaoDeveEditarCurso_Inexistente()
    {
        var comando = new EditarCursoComando(Guid.NewGuid(), "Curso", "Descrição");

        var resultado = await _handler.Handle(comando, CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Contains("curso não encontrado", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve editar curso com nome vazio")]
    public async Task NaoDeveEditarCurso_ComNomeVazio()
    {
        var curso = new Curso("Curso Original", new ConteudoProgramatico("Desc", "Obj"), "Teste");
        await _repositorio.Adicionar(curso);

        var comando = new EditarCursoComando(curso.Id, "", "Nova descrição");

        var resultado = await _handler.Handle(comando, CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Contains("nome", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve editar curso com descrição vazia")]
    public async Task NaoDeveEditarCurso_ComDescricaoVazia()
    {
        var curso = new Curso("Curso Original", new ConteudoProgramatico("Desc", "Obj"), "Teste");
        await _repositorio.Adicionar(curso);

        var comando = new EditarCursoComando(curso.Id, "Novo Nome", "");

        var resultado = await _handler.Handle(comando, CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Contains("descrição", resultado.Mensagem.ToLower());
    }
}
