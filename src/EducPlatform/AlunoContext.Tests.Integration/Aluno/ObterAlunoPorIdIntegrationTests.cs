using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;

namespace AlunoContext.Tests.Integration.Aluno;

public class ObterAlunoPorIdIntegrationTests
{
    [Fact(DisplayName = "Deve retornar aluno existente pelo ID")]
    public void DeveObterAluno_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario);
        cadastrarHandler.Handle(new CadastrarAlunoComando("Filipe", "filipe@email.com"));
        var alunoCriado = contexto.Alunos.First();

        // Act
        var aluno = repositorio.ObterPorId(alunoCriado.Id);

        // Assert
        Assert.NotNull(aluno);
        Assert.Equal("Filipe", aluno!.Nome);
        Assert.Equal("filipe@email.com", aluno.Email);
    }
}
