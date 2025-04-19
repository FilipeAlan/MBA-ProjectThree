using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Alunos;

public class CadastrarAlunosPerformanceTests
{
    [Fact(DisplayName = "Deve cadastrar 1000 alunos em menos de 5 segundos")]
    public async Task DeveCadastrarAlunosRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);
        var handler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        for (int i = 0; i < 1000; i++)
        {
            var comando = new CadastrarAlunoComando($"Aluno {i}", $"aluno{i}@email.com");
            await handler.Handle(comando);
        }

        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.Elapsed.TotalSeconds < 5,
            $"Cadastro demorou {stopwatch.Elapsed.TotalSeconds} segundos");
    }
}
