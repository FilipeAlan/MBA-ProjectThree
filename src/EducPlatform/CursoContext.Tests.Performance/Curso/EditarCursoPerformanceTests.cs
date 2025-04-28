using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Commands.EditarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace CursoContext.Tests.Performance.Curso;

public class EditarCursoPerformanceTests
{
    [Fact(DisplayName = "Deve editar 1000 cursos em menos de 5 segundos")]
    public async Task DeveEditarVariosCursosRapidamente()
    {
        // aaa Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);
        var cadastrarHandler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);

        for (int i = 0; i < 1000; i++)
            await cadastrarHandler.Handle(new CadastrarCursoComando($"Curso {i}", "Descricao"));

        var cursos = contexto.Cursos.ToList();
        var editarHandler = new EditarCursoHandler(repositorio, unitOfWork, usuario);
        var stopwatch = Stopwatch.StartNew();

        // aaa Act
        foreach (var curso in cursos)
        {
            var comando = new EditarCursoComando(curso.Id, curso.Nome + " Editado", "Nova Descricao");
            await editarHandler.Handle(comando);
        }

        stopwatch.Stop();

        // aaa Assert
        Assert.True(stopwatch.Elapsed.TotalSeconds < 5, $"Edição demorou {stopwatch.Elapsed.TotalSeconds} segundos.");
    }
}
