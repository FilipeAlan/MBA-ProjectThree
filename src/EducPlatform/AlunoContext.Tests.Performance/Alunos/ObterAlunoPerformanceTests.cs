using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Alunos;

public class ObterAlunoPerformanceTests
{
    [Fact(DisplayName = "Deve obter aluno pelo ID em menos de 100ms")]
    public void DeveBuscarAlunoPorIdRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var handler = new CadastrarAlunoHandler(repositorio, usuario);

        handler.Handle(new CadastrarAlunoComando("Filipe", "filipe@email.com"));
        var aluno = contexto.Alunos.First();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        var resultado = repositorio.ObterPorId(aluno.Id);

        stopwatch.Stop();

        // Assert
        Assert.NotNull(resultado);
        Assert.True(stopwatch.Elapsed.TotalMilliseconds < 100,
            $"Busca demorou {stopwatch.Elapsed.TotalMilliseconds}ms");
    }
}
