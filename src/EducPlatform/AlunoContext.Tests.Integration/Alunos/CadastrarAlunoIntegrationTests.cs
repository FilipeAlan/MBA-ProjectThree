using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;

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
        var unitOfWork = new UnitOfWork(contexto);
        var handler = new CadastrarAlunoHandler(repositorio, usuarioContexto, unitOfWork);

        var comando = new CadastrarAlunoComando("Filipe", "filipe@email.com");

        // Act
        var resultado = await handler.Handle(comando, CancellationToken.None);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Equal(1, contexto.Alunos.Count());
    }
}
