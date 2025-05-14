using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace CursoContext.Tests.Performance.Curso;

public class ObterCursoPerformanceTests
{
    [Fact(DisplayName = "Deve obter curso por ID em menos de 100ms")]
    public async Task DeveObterCursoRapidamente()
    {
        // aaa Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);
        var handler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);

        await handler.Handle(new CadastrarCursoComando("Curso Rápido", "Performance"), CancellationToken.None);
        var curso = contexto.Cursos.First();

        var stopwatch = Stopwatch.StartNew();

        // aaa Act
        var resultado = await repositorio.ObterPorId(curso.Id);

        stopwatch.Stop();

        // aaa Assert
        Assert.NotNull(resultado);
        Assert.True(stopwatch.Elapsed.TotalMilliseconds < 100, $"Obtenção demorou {stopwatch.Elapsed.TotalMilliseconds}ms.");
    }
}
