using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Commands.DeletarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;

namespace AlunoContext.Tests.Integration.Aluno;

public class DeletarAlunoIntegrationTests
{
    [Fact(DisplayName = "Deve remover aluno existente do banco de dados")]
    public void DeveRemoverAluno_ComSucesso()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario);
        var alunoCriado = cadastrarHandler.Handle(new CadastrarAlunoComando("Filipe", "filipe@email.com"));
        var aluno = contexto.Alunos.First();

        var deletarHandler = new DeletarAlunoHandler(repositorio, usuario);
        var comando = new DeletarAlunoComando(aluno.Id);

        // Act
        var resultado = deletarHandler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        Assert.Empty(contexto.Alunos.ToList());
    }
}
