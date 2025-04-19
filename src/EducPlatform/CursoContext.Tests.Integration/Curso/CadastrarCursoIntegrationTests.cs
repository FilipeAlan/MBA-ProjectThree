using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;

namespace CursoContext.Tests.Integration.Curso;

public class CadastrarCursoIntegrationTests
{
    [Fact(DisplayName = "Deve cadastrar curso e persistir no banco de dados")]
    public async Task DeveCadastrarCurso_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWorkFake();
        var handler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);

        var comando = new CadastrarCursoComando("Curso de TDD", "Aprenda a testar antes de codar");

        // Act
        var resultado = await handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Single(contexto.Cursos);
        Assert.Equal("Curso de TDD", contexto.Cursos.First().Nome);
    }
}
