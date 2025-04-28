using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Queries.ListarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;

namespace CursoContext.Tests.Integration.Curso;

public class ListarCursosIntegrationTests
{
    [Fact(DisplayName = "Deve retornar todos os cursos cadastrados")]
    public async Task DeveRetornarTodosCursos()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);

        var cadastrarHandler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);
        await cadastrarHandler.Handle(new CadastrarCursoComando("Curso A", "Descrição A"));
        await cadastrarHandler.Handle(new CadastrarCursoComando("Curso B", "Descrição B"));

        var listarHandler = new ListarCursosHandler(repositorio);

        // Act
        var cursos = await listarHandler.Handle(new ListarCursosQuery());

        // Assert
        Assert.Equal(2, cursos.Count);
        Assert.Contains(cursos, c => c.Nome == "Curso A");
        Assert.Contains(cursos, c => c.Nome == "Curso B");
    }
}
