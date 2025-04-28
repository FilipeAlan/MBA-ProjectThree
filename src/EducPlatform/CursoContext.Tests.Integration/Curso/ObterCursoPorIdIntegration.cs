using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;

namespace CursoContext.Tests.Integration.Curso;

public class ObterCursoPorIdIntegrationTests
{
    [Fact(DisplayName = "Deve retornar curso existente pelo ID")]
    public async Task DeveRetornarCursoPorId()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);
        var handler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);

        await handler.Handle(new CadastrarCursoComando("Curso A", "Descrição A"));

        var cursoId = contexto.Cursos.First().Id;

        // Act
        var curso = await repositorio.ObterPorId(cursoId);

        // Assert
        Assert.NotNull(curso);
        Assert.Equal("Curso A", curso.Nome);
    }
}
