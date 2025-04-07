using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Integration.Shared;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Aluno;

public class EditarAlunoPerformanceTests
{
    [Fact(DisplayName = "Deve editar 1000 alunos em menos de 3 segundos")]
    public void DeveEditarVariosAlunosRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario);

        // Cria 1000 alunos
        for (int i = 0; i < 1000; i++)
        {
            var comandoCadastro = new CadastrarAlunoComando($"Aluno {i}", $"aluno{i}@email.com");
            cadastrarHandler.Handle(comandoCadastro);
        }

        var alunos = contexto.Alunos.ToList();
        var editarHandler = new EditarAlunoHandler(repositorio, usuario);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Act
        foreach (var aluno in alunos)
        {
            var comandoEdicao = new EditarAlunoComando(
                aluno.Id,
                $"{aluno.Nome} Editado",
                aluno.Email.Replace("@", ".editado@")
            );

            editarHandler.Handle(comandoEdicao);
        }

        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.Elapsed.TotalSeconds < 3,
            $"Edição demorou {stopwatch.Elapsed.TotalSeconds} segundos para 1000 alunos.");
    }
}
