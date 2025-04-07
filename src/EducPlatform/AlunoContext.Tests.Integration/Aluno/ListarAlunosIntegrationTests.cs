using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;

namespace AlunoContext.Tests.Integration.Aluno;

public class ListarAlunosIntegrationTests
{
    [Fact(DisplayName = "Deve retornar todos os alunos cadastrados")]
    public void DeveListarTodosOsAlunos()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario);
        cadastrarHandler.Handle(new CadastrarAlunoComando("Filipe", "filipe@email.com"));
        cadastrarHandler.Handle(new CadastrarAlunoComando("João", "joao@email.com"));

        // Act
        var alunos = repositorio.Listar();

        // Assert
        Assert.Equal(2, alunos.Count);
        Assert.Contains(alunos, a => a.Nome == "Filipe");
        Assert.Contains(alunos, a => a.Nome == "João");
    }
}
