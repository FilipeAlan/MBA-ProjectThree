using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;

namespace AlunoContext.Tests.Integration.Alunos;

public class ObterAlunoPorIdIntegrationTests
{
    [Fact(DisplayName = "Deve retornar aluno existente pelo ID")]
    public async Task DeveObterAluno_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario);
        await cadastrarHandler.Handle(new CadastrarAlunoComando("Filipe", "filipe@email.com"));
        var alunoCriado = contexto.Alunos.First();

        // Act
        var aluno = await repositorio.ObterPorId(alunoCriado.Id);

        // Assert
        Assert.NotNull(aluno);
        Assert.Equal("Filipe", aluno!.Nome);
        Assert.Equal("filipe@email.com", aluno.Email);
    }
}
