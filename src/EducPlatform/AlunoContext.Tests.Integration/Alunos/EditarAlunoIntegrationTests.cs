using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Commands.EditarAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;

namespace AlunoContext.Tests.Integration.Alunos;

public class EditarAlunoIntegrationTests
{
    [Fact(DisplayName = "Deve editar o nome e email do aluno no banco de dados")]
    public async Task DeveEditarAluno_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);
        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork);
        var comandoCadastro = new CadastrarAlunoComando("Filipe", "filipe@email.com");
        await cadastrarHandler.Handle(comandoCadastro);
        var aluno = contexto.Alunos.First();

        var editarHandler = new EditarAlunoHandler(repositorio, usuario, unitOfWork);
        var comandoEdicao = new EditarAlunoComando(aluno.Id, "Novo Nome", "novo@email.com");

        // Act
        var resultado = await editarHandler.Handle(comandoEdicao);
        var alunoEditado = contexto.Alunos.First();

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Equal("Novo Nome", alunoEditado.Nome);
        Assert.Equal("novo@email.com", alunoEditado.Email);
    }
}

