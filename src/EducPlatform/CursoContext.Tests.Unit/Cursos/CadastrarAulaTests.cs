using CursoContext.Application.Commands.CadastrarAula;
using CursoContext.Domain.Aggregates;
using CursoContext.Tests.Shared.Fakes;
using CursoContext.Tests.Unit.Fakes;

namespace CursoContext.Tests.Unit.Cursos;

public class CadastrarAulaTests
{
    private readonly CursoRepositorioFake _repositorio;
    private readonly UsuarioContextoFake _usuario;
    private readonly UnitOfWorkFake _unitOfWork;
    private readonly CadastrarAulaHandler _handler;

    public CadastrarAulaTests()
    {
        _repositorio = new CursoRepositorioFake();
        _usuario = new UsuarioContextoFake();
        _unitOfWork = new UnitOfWorkFake();
        _handler = new CadastrarAulaHandler(_repositorio, _usuario, _unitOfWork);
    }

    [Fact(DisplayName = "Deve adicionar aula ao curso com sucesso")]
    public async Task DeveCadastrarAula_ComSucesso()
    {
        // Arrange
        var curso = new Curso("Curso A", new("Descricao", "Objetivos"), "teste");
        await _repositorio.Adicionar(curso);

        var comando = new CadastrarAulaComando(curso.Id, "Título da Aula", "Conteúdo da Aula");

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso); 
      
        var cursoAtualizado = await _repositorio.ObterPorId(curso.Id);
      
        Assert.NotNull(cursoAtualizado);  // O curso não deve ser nulo
        Assert.NotEmpty(cursoAtualizado!.Aulas);  // A lista de aulas deve ter pelo menos uma aula associada        
        Assert.Single(cursoAtualizado.Aulas);  // O curso deve ter exatamente uma aula      
        Assert.Equal("Título da Aula", cursoAtualizado.Aulas.FirstOrDefault()?.Titulo); 
    }


    [Fact(DisplayName = "Não deve adicionar aula a curso inexistente")]
    public async Task NaoDeveCadastrarAula_CursoInexistente()
    {
        var comando = new CadastrarAulaComando(Guid.NewGuid(), "Título", "Conteúdo");

        var resultado = await _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("curso não encontrado", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve adicionar aula com título vazio")]
    public async Task NaoDeveCadastrarAula_TituloVazio()
    {
        var curso = new Curso("Curso A", new("Descricao", "Objetivos"), "teste");
        await _repositorio.Adicionar(curso);

        var comando = new CadastrarAulaComando(curso.Id, "", "Conteúdo");

        var resultado = await _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("título", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve adicionar aula com conteúdo vazio")]
    public async Task NaoDeveCadastrarAula_ConteudoVazio()
    {
        var curso = new Curso("Curso A", new("Descricao", "Objetivos"), "teste");
        await _repositorio.Adicionar(curso);

        var comando = new CadastrarAulaComando(curso.Id, "Título", "");

        var resultado = await _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("conteúdo", resultado.Mensagem.ToLower());
    }
}
