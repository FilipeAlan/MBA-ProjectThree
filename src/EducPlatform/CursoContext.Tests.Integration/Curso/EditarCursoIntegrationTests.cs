using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Commands.EditarCurso;
using CursoContext.Infrastructure.Context;
using CursoContext.Infrastructure.Repositories;
using CursoContext.Tests.Integration.Shared;
using CursoContext.Tests.Shared.Fakes;

namespace CursoContext.Tests.Integration.Curso;

public class EditarCursoIntegrationTests
{
    [Fact(DisplayName = "Deve editar curso existente no banco de dados")]
    public async Task DeveEditarCurso_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new CursoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);


        var cadastrarHandler = new CadastrarCursoHandler(repositorio, usuario, unitOfWork);
        await cadastrarHandler.Handle(new CadastrarCursoComando("Curso Antigo", "Descrição antiga"));

        var curso = contexto.Cursos.First();
        var editarHandler = new EditarCursoHandler(repositorio, unitOfWork, usuario);
        var comando = new EditarCursoComando(curso.Id, "Curso Atualizado", "Nova descrição");

        // Act
        var resultado = await editarHandler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        var cursoAtualizado = contexto.Cursos.First();
        Assert.Equal("Curso Atualizado", cursoAtualizado.Nome);
        Assert.Equal("Nova descrição", cursoAtualizado.Conteudo.Descricao);
    }
}
