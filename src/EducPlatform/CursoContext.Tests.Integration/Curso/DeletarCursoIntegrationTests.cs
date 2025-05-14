using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Commands.DeletarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;

namespace CursoContext.Tests.Integration.Curso;

public class DeletarCursoIntegrationTests
{
    [Fact(DisplayName = "Deve deletar curso do banco de dados")]
    public async Task DeveDeletarCurso_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);

        var cadastrarHandler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);
        await cadastrarHandler.Handle(new CadastrarCursoComando("Curso de TDD", "Aprenda TDD"),CancellationToken.None);       

        var curso = contexto.Cursos.First();

        var deletarHandler = new DeletarCursoHandler(repositorio, unitOfWork);

        // Act
        var resultado = await deletarHandler.Handle(new DeletarCursoComando(curso.Id),CancellationToken.None);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Empty(contexto.Cursos.ToList());
    }
}
