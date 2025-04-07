using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Aluno;

public class CadastrarAlunosPerformanceTests
{
    [Fact(DisplayName = "Deve cadastrar 1000 alunos em menos de 3 segundos")]
    public void DeveCadastrarAlunosRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var handler = new CadastrarAlunoHandler(repositorio, usuario);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        for (int i = 0; i < 1000; i++)
        {
            var comando = new CadastrarAlunoComando($"Aluno {i}", $"aluno{i}@email.com");
            handler.Handle(comando);
        }

        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.Elapsed.TotalSeconds < 3,
            $"Cadastro demorou {stopwatch.Elapsed.TotalSeconds} segundos");
    }
}
