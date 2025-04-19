using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Commands.EditarAluno;
using AlunoContext.Infrastructure.Context;
using AlunoContext.Infrastructure.Repositories;
using AlunoContext.Tests.Integration.Shared;
using AlunoContext.Tests.Shared.Fakes;
using System.Diagnostics;

namespace AlunoContext.Tests.Performance.Alunos;

public class EditarAlunoPerformanceTests
{
    [Fact(DisplayName = "Deve editar 1000 alunos em menos de 5 segundos")]
    public async Task DeveEditarVariosAlunosRapidamente()
    {
        // Arrange
        using var contexto = TestDbContextFactory.CriarContexto();
        var repositorio = new AlunoRepository(contexto);
        var usuario = new UsuarioContextoFake();
        var unitOfWork = new UnitOfWork(contexto);

        var cadastrarHandler = new CadastrarAlunoHandler(repositorio, usuario, unitOfWork);

        for (int i = 0; i < 1000; i++)
        {
            var comandoCadastro = new CadastrarAlunoComando($"Aluno {i}", $"aluno{i}@email.com");
            await cadastrarHandler.Handle(comandoCadastro);
        }

        var alunos = contexto.Alunos.ToList();
        var editarHandler = new EditarAlunoHandler(repositorio, usuario, unitOfWork);

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

            await editarHandler.Handle(comandoEdicao);
        }

        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.Elapsed.TotalSeconds < 5,
            $"Edição demorou {stopwatch.Elapsed.TotalSeconds} segundos para 1000 alunos.");
    }
}
