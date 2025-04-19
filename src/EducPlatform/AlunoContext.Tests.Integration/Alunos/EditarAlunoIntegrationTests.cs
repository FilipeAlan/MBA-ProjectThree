using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Application.Commands.EditarAluno;

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

            var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario);
            var comandoCadastro = new CadastrarAlunoComando("Filipe", "filipe@email.com");
            await cadastrarHandler.Handle(comandoCadastro);
            var aluno = contexto.Alunos.First();

            var editarHandler = new EditarAlunoHandler(repositorio, usuario);
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

