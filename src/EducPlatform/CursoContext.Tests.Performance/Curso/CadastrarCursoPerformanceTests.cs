using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace CursoContext.Tests.Performance.Curso;

public class CadastrarCursoPerformanceTests
{
    [Fact(DisplayName = "Deve cadastrar 1000 cursos em menos de 10 segundos")]
    public async Task DeveCadastrarCursosRapidamente()
    {
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);

        var handler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < 1000; i++)
        {
            var comando = new CadastrarCursoComando($"Curso {i}", $"Descrição do curso {i}");
            await handler.Handle(comando);
        }

        stopwatch.Stop();

        Assert.True(stopwatch.Elapsed.TotalSeconds < 10,
            $"Cadastro demorou {stopwatch.Elapsed.TotalSeconds} segundos");
    }
}
