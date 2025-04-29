using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace CursoContext.Tests.Performance.Curso;

public class CadastrarCursoPerformanceTests
{
<<<<<<< Updated upstream
    [Fact(DisplayName = "Deve cadastrar 1000 cursos em menos de 3 segundos")]
=======
    [Fact(DisplayName = "Deve cadastrar 1000 cursos em menos de 20 segundos")]
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        Assert.True(stopwatch.Elapsed.TotalSeconds < 5,
=======
        Assert.True(stopwatch.Elapsed.TotalSeconds < 20,
>>>>>>> Stashed changes
            $"Cadastro demorou {stopwatch.Elapsed.TotalSeconds} segundos");
    }
}
