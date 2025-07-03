using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Queries.ListarAlunos;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;

namespace AlunoContext.Tests.Integration.Alunos;

public class ListarAlunosIntegrationTests
{
    [Fact(DisplayName = "Deve retornar todos os alunos cadastrados")]
    public async Task DeveListarTodosOsAlunos()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork);
        await cadastrarHandler.Handle(new CadastrarAlunoComando(Guid.NewGuid(), "Filipe", "filipe@email.com"), CancellationToken.None);
        await cadastrarHandler.Handle(new CadastrarAlunoComando(Guid.NewGuid(), "João", "joao@email.com"),CancellationToken.None);

        var listarHandler = new ListarAlunosHandler(repositorio);

        // Act
        var alunos = await listarHandler.Handle(new ListarAlunosQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(2, alunos.Count);
        Assert.Contains(alunos, a => a.Nome == "Filipe");
        Assert.Contains(alunos, a => a.Nome == "João");
    }
}
