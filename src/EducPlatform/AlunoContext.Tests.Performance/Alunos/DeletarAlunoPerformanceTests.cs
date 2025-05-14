using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Commands.DeletarAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Alunos;

public class DeletarAlunoPerformanceTests
{
    [Fact(DisplayName = "Deve deletar 1000 alunos em menos de 10 segundos")]
    public async Task DeveDeletarAlunosRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var unitOfWork = new UnitOfWork(contexto); // ✅ necessário agora
        var usuario = new UsuarioContextoFake();

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork);

        for (int i = 0; i < 1000; i++)
        {
            var comando = new CadastrarAlunoComando($"Aluno {i}", $"aluno{i}@email.com");
            await cadastrarHandler.Handle(comando, CancellationToken.None);
        }

        var alunos = contexto.Alunos.Select(a => a.Id).ToList();

        var deletarHandler = new DeletarAlunoHandler(repositorio, unitOfWork); 

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        foreach (var id in alunos)
        {
            var comando = new DeletarAlunoComando(id);
            await deletarHandler.Handle(comando);
        }

        stopwatch.Stop();

        // Assert
        Assert.Equal(0, contexto.Alunos.Count());
        Assert.True(stopwatch.Elapsed.TotalSeconds < 10,
            $"Remoção demorou {stopwatch.Elapsed.TotalSeconds} segundos");
    }
}
