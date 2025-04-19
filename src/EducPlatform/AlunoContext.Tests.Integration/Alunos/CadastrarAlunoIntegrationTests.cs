using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;

namespace AlunoContext.Tests.Integration.Alunos;

public class CadastrarAlunoIntegrationTests
{
    [Fact(DisplayName = "Deve cadastrar aluno e persistir no banco de dados")]
    public async Task DeveCadastrarAluno_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuarioContexto = new UsuarioContextoFake();
        var handler = new CadastrarAlunoHandler(repositorio, usuarioContexto);

        var comando = new CadastrarAlunoComando("Filipe", "filipe@email.com");

        // Act
        var resultado = await handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Equal(1, contexto.Alunos.Count());
    }
}
