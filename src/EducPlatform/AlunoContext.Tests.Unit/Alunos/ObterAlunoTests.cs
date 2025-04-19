using AlunoContext.Application.Queries.ObterAluno;
using AlunoContext.Tests.Shared.Builders;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Tests.Unit.Alunos;

public class ObterAlunoTests
{
    private readonly AlunoRepositorioFake _repositorioFake;
    private readonly ObterAlunoHandler _handler;

    public ObterAlunoTests()
    {
        _repositorioFake = new AlunoRepositorioFake();
        _handler = new ObterAlunoHandler(_repositorioFake);
    }

    [Fact(DisplayName = "Deve retornar aluno quando o ID existir")]
    public async Task DeveObterAluno_Existente()
    {
        // Arrange
        var aluno = AlunoBuilder.Novo()
            .ComNome("Filipe")
            .ComEmail("filipe@email.com")
            .Construir();

        await _repositorioFake.Adicionar(aluno);
        var query = new ObterAlunoQuery(aluno.Id);

        // Act
        var resultado = await _handler.Handle(query);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(aluno.Id, resultado.Id);
        Assert.Equal(aluno.Nome, resultado.Nome);
        Assert.Equal(aluno.Email, resultado.Email);
    }

    [Fact(DisplayName = "Deve retornar nulo quando o ID não existir")]
    public async Task DeveObterAluno_NaoExistente()
    {
        // Arrange
        var query = new ObterAlunoQuery(Guid.NewGuid());

        // Act
        var resultado = await _handler.Handle(query);

        // Assert
        Assert.Null(resultado);
    }
}
