using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Testes.Shared.Helpers;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Testes.Unit.Aluno;

public class CadastrarAlunoTests
{
    private readonly RepositorioFake _repositorioFake;
    private readonly UsuarioContextoFake _usuarioFake;
    private readonly CadastrarAlunoHandler _handler;

    public CadastrarAlunoTests()
    {
        _repositorioFake = new RepositorioFake();
        _usuarioFake = new UsuarioContextoFake();
        _handler = new CadastrarAlunoHandler(_repositorioFake, _usuarioFake);
    }

    [Fact(DisplayName = "Deve cadastrar aluno quando os dados forem válidos")]
    public void DeveCadastrarAluno_ComDadosValidos()
    {
        var comando = GeradorDeComando.CriarAlunoValido();

        var resultado = _handler.Handle(comando);

        Assert.True(resultado.Sucesso);
        Assert.Single(_repositorioFake.Alunos);
        Assert.Equal(comando.Nome, _repositorioFake.Alunos[0].Nome);
    }

    [Fact(DisplayName = "Não deve cadastrar aluno quando o nome estiver vazio")]
    public void NaoDeveCadastrarAluno_ComNomeVazio()
    {
        var comando = GeradorDeComando.CriarAlunoComNomeVazio();

        var resultado = _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("nome", resultado.Mensagem.ToLower());
        Assert.Empty(_repositorioFake.Alunos);
    }

    [Fact(DisplayName = "Não deve cadastrar aluno quando o e-mail for inválido")]
    public void NaoDeveCadastrarAluno_ComEmailInvalido()
    {
        var comando = GeradorDeComando.CriarAlunoComEmailInvalido();

        var resultado = _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("e-mail", resultado.Mensagem.ToLower());
        Assert.Empty(_repositorioFake.Alunos);
    }
}
