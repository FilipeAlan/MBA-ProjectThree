using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Commands.DeletarAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;

namespace AlunoContext.Tests.Integration.Alunos;

public class DeletarAlunoIntegrationTests
{
    [Fact(DisplayName = "Deve remover aluno existente do banco de dados")]
    public async Task DeveRemoverAluno_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var unitOfWork = new UnitOfWork(contexto);
        var usuario = new UsuarioContextoFake();

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork); // se o handler de cadastrar também tiver unitOfWork
        await cadastrarHandler.Handle(new CadastrarAlunoComando("Filipe", "filipe@email.com"),CancellationToken.None);

        var aluno = contexto.Alunos.First();

        var deletarHandler = new DeletarAlunoHandler(repositorio, unitOfWork);
        var comando = new DeletarAlunoComando(aluno.Id);

        // Act
        var resultado = await deletarHandler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Empty(contexto.Alunos.ToList());
    }
}
