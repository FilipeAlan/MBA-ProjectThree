using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Queries.ObterAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;

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
        var unitOfWork = new UnitOfWork(contexto);

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork);
        await cadastrarHandler.Handle(new CadastrarAlunoComando("Filipe", "filipe@email.com"), CancellationToken.None);
        var alunoCriado = contexto.Alunos.First();

        var obterHandler = new ObterAlunoHandler(repositorio);
        var query = new ObterAlunoQuery(alunoCriado.Id);

        // Act
        var aluno = await obterHandler.Handle(query);

        // Assert
        Assert.NotNull(aluno);
        Assert.Equal("Filipe", aluno!.Nome);
        Assert.Equal("filipe@email.com", aluno.Email);
    }
}
