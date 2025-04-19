using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Alunos;

public class ListarAlunosPerformanceTests
{
    [Fact(DisplayName = "Deve listar 1000 alunos em menos de 500ms")]
    public async Task DeveListarAlunosRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var handler = new CadastrarAlunoHandler(repositorio, usuario);

        for (int i = 0; i < 1000; i++)
        {
            await handler.Handle(new CadastrarAlunoComando($"Aluno {i}", $"aluno{i}@email.com"));
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        var alunos = await repositorio.Listar();

        stopwatch.Stop();

        // Assert
        Assert.Equal(1000, alunos.Count);
        Assert.True(stopwatch.Elapsed.TotalMilliseconds < 500,
            $"Listagem demorou {stopwatch.Elapsed.TotalMilliseconds}ms");
    }
}
