using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Commands.DeletarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace CursoContext.Tests.Performance.Curso;

public class DeletarCursoPerformanceTests
{
    [Fact(DisplayName = "Deve deletar 1000 cursos em menos de 10 segundos")]
    public async Task DeveDeletarVariosCursosRapidamente()
    {
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);

        var cadastrarHandler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);

        for (int i = 0; i < 1000; i++)
            await cadastrarHandler.Handle(new CadastrarCursoComando($"Curso {i}", "Descricao"));

        var cursos = contexto.Cursos.ToList();
        var deletarHandler = new DeletarCursoHandler(repositorio, unitOfWork);

        var stopwatch = Stopwatch.StartNew();

        foreach (var curso in cursos)
            await deletarHandler.Handle(new DeletarCursoComando(curso.Id));

        stopwatch.Stop();

        Assert.True(stopwatch.Elapsed.TotalSeconds < 10, $"Remoção demorou {stopwatch.Elapsed.TotalSeconds} segundos.");
        Assert.Empty(contexto.Cursos.ToList());
    }
}
