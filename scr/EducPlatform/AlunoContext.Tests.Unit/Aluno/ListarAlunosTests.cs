using AlunoContext.Application.Queries.ListarAlunos;
using AlunoContext.Testes.Shared.Builders;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Testes.Unit.Aluno;

public class ListarAlunosTests
{
    private readonly RepositorioFake _repositorioFake;

    public ListarAlunosTests()
    {
        _repositorioFake = new RepositorioFake();
    }

    [Fact(DisplayName = "Deve retornar todos os alunos cadastrados")]
    public void DeveListarTodosOsAlunos()
    {
        var aluno1 = AlunoBuilder.Novo().ComNome("Filipe").Construir();
        var aluno2 = AlunoBuilder.Novo().ComNome("João").Construir();

        _repositorioFake.Adicionar(aluno1);
        _repositorioFake.Adicionar(aluno2);

        var handler = new ListarAlunosHandler(_repositorioFake);
        var resultado = handler.Handle(new ListarAlunosQuery());

        Assert.Equal(2, resultado.Count);
        Assert.Contains(resultado, x => x.Nome == "Filipe");
        Assert.Contains(resultado, x => x.Nome == "João");
    }

    [Fact(DisplayName = "Deve retornar lista vazia quando não houver alunos cadastrados")]
    public void DeveRetornarListaVazia_QuandoNaoExistiremAlunos()
    {
        var handler = new ListarAlunosHandler(_repositorioFake);

        var resultado = handler.Handle(new ListarAlunosQuery());

        Assert.NotNull(resultado); // deve retornar lista, não null
        Assert.Empty(resultado);   // deve estar vazia
    }
}
