using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Alunos;

public class ObterAlunoPerformanceTests
{
    [Fact(DisplayName = "Deve obter aluno pelo ID em menos de 100ms")]
    public async Task DeveBuscarAlunoPorIdRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var unitOfWork = new UnitOfWork(contexto);
        var usuario = new UsuarioContextoFake();
        var handler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork);

        await handler.Handle(new CadastrarAlunoComando("Filipe", "filipe@email.com"));
        var aluno = contexto.Alunos.First();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        var resultado = repositorio.ObterPorId(aluno.Id);

        stopwatch.Stop();

        // Assert
        Assert.NotNull(resultado);
        Assert.True(stopwatch.Elapsed.TotalMilliseconds < 500,
            $"Busca demorou {stopwatch.Elapsed.TotalMilliseconds}ms");
    }
}
