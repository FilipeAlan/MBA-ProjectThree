using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Shared.Helpers;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Tests.Unit.Alunos;

public class CadastrarAlunoTests
{
    private readonly AlunoRepositorioFake _repositorioFake;
    private readonly UsuarioContextoFake _usuarioFake;
    private readonly CadastrarAlunoHandler _handler;

    public CadastrarAlunoTests()
    {
        _repositorioFake = new AlunoRepositorioFake();
        _usuarioFake = new UsuarioContextoFake();
        _handler = new CadastrarAlunoHandler(_repositorioFake, _usuarioFake);
    }

    [Fact(DisplayName = "Deve cadastrar aluno quando os dados forem válidos")]
    public async Task DeveCadastrarAluno_ComDadosValidos()
    {
        var comando = GeradorDeComando.CriarAlunoValido();

        var resultado = await _handler.Handle(comando);

        Assert.True(resultado.Sucesso);
        Assert.Single(_repositorioFake.Alunos);
        Assert.Equal(comando.Nome, _repositorioFake.Alunos[0].Nome);
    }

    [Fact(DisplayName = "Não deve cadastrar aluno quando o nome estiver vazio")]
    public async Task NaoDeveCadastrarAluno_ComNomeVazio()
    {
        var comando = GeradorDeComando.CriarAlunoComNomeVazio();

        var resultado = await _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("nome", resultado.Mensagem.ToLower());
        Assert.Empty(_repositorioFake.Alunos);
    }

    [Fact(DisplayName = "Não deve cadastrar aluno quando o e-mail for inválido")]
    public async Task NaoDeveCadastrarAluno_ComEmailInvalido()
    {
        var comando = GeradorDeComando.CriarAlunoComEmailInvalido();

        var resultado = await _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("e-mail", resultado.Mensagem.ToLower());
        Assert.Empty(_repositorioFake.Alunos);
    }
}
