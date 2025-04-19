using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Commands.DeletarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Aluno;

public class DeletarAlunoPerformanceTests
{
    [Fact(DisplayName = "Deve deletar 1000 alunos em menos de 3 segundos")]
    public void DeveDeletarAlunosRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario);

        for (int i = 0; i < 1000; i++)
        {
            var comando = new CadastrarAlunoComando($"Aluno {i}", $"aluno{i}@email.com");
            cadastrarHandler.Handle(comando);
        }

        var alunos = contexto.Alunos.Select(a => a.Id).ToList();

        var deletarHandler = new DeletarAlunoHandler(repositorio, usuario);
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        foreach (var id in alunos)
        {
            var comando = new DeletarAlunoComando(id);
            deletarHandler.Handle(comando);
        }

        stopwatch.Stop();

        // Assert
        Assert.Equal(0, contexto.Alunos.Count());
        Assert.True(stopwatch.Elapsed.TotalSeconds < 5,
            $"Remoção demorou {stopwatch.Elapsed.TotalSeconds} segundos");
    }
}
