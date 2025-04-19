using AlunoContext.Application.Queries.ListarAlunos;
using AlunoContext.Tests.Shared.Builders;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Tests.Unit.Alunos;

public class ListarAlunosTests
{
    private readonly AlunoRepositorioFake _repositorioFake;

    public ListarAlunosTests()
    {
        _repositorioFake = new AlunoRepositorioFake();
    }

    [Fact(DisplayName = "Deve retornar todos os alunos cadastrados")]
    public async Task DeveListarTodosOsAlunos()
    {
        var aluno1 = AlunoBuilder.Novo().ComNome("Filipe").Construir();
        var aluno2 = AlunoBuilder.Novo().ComNome("João").Construir();

        await _repositorioFake.Adicionar(aluno1);
        await _repositorioFake.Adicionar(aluno2);

        var handler = new ListarAlunosHandler(_repositorioFake);
        var resultado = await handler.Handle(new ListarAlunosQuery());

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, x => x.Nome == "Filipe");
        Assert.Contains(resultado, x => x.Nome == "João");
    }

    [Fact(DisplayName = "Deve retornar lista vazia quando não houver alunos cadastrados")]
    public async Task DeveRetornarListaVazia_QuandoNaoExistiremAlunos()
    {
        var handler = new ListarAlunosHandler(_repositorioFake);

        var resultado = await handler.Handle(new ListarAlunosQuery());

        Assert.NotNull(resultado); // deve retornar lista, não null
        Assert.Empty(resultado);   // deve estar vazia
    }
}
