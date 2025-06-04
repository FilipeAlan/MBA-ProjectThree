using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Alunos;

public class ListarAlunosPerformanceTests
{
    [Fact(DisplayName = "Deve listar 1000 alunos em menos de 2000ms")]
    public async Task DeveListarAlunosRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);
        var handler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork);

        for (int i = 0; i < 1000; i++)
        {
            await handler.Handle(new CadastrarAlunoComando(Guid.NewGuid(), $"Aluno {i}", $"aluno{i}@email.com"), CancellationToken.None);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        var alunos = await repositorio.Listar();

        stopwatch.Stop();

        // Assert
        Assert.Equal(1000, alunos.Count);
        Assert.True(stopwatch.Elapsed.TotalMilliseconds < 2000,
            $"Listagem demorou {stopwatch.Elapsed.TotalMilliseconds}ms");
    }
}
