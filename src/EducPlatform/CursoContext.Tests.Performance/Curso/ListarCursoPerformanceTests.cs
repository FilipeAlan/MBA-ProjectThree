using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace CursoContext.Tests.Performance.Curso;

public class ListarCursoPerformanceTests
{
	[Fact(DisplayName = "Deve listar 1000 cursos em menos de 2000ms")]
	public async Task DeveListarCursosRapidamente()
	{
		// Arrange
		using var contexto = TestDbContextFactory.CriarContexto();
		var repositorio = new CursoRepository(contexto);
		var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);
        var handler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);

		for (int i = 0; i < 1000; i++)
		{
			await handler.Handle(new CadastrarCursoComando($"Curso {i}", $"Descrição {i}"));
		}

		var stopwatch = new Stopwatch();
		stopwatch.Start();

		// Act
		var cursos = await repositorio.Listar();

		stopwatch.Stop();

		// Assert
		Assert.Equal(1000, cursos.Count);
		Assert.True(stopwatch.Elapsed.TotalMilliseconds < 2000,
			$"Listagem demorou {stopwatch.Elapsed.TotalMilliseconds}ms");
	}
}
