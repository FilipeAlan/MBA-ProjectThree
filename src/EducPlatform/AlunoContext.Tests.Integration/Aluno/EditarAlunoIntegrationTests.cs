using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;

namespace AlunoContext.Tests.Integration.Aluno
{
    public class EditarAlunoIntegrationTests
    {
        [Fact(DisplayName = "Deve editar o nome e email do aluno no banco de dados")]
        public void DeveEditarAluno_ComSucesso()
        {
            // Arrange
            using var contexto = TestDbContextFactory.CriarContexto();
            var repositorio = new AlunoRepository(contexto);
            var usuario = new UsuarioContextoFake();

            var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario);
            var comandoCadastro = new CadastrarAlunoComando("Filipe", "filipe@email.com");
            cadastrarHandler.Handle(comandoCadastro);
            var aluno = contexto.Alunos.First();

            var editarHandler = new EditarAlunoHandler(repositorio, usuario);
            var comandoEdicao = new EditarAlunoComando(aluno.Id, "Novo Nome", "novo@email.com");

            // Act
            var resultado = editarHandler.Handle(comandoEdicao);
            var alunoEditado = contexto.Alunos.First();

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal("Novo Nome", alunoEditado.Nome);
            Assert.Equal("novo@email.com", alunoEditado.Email);
        }
    }
}
