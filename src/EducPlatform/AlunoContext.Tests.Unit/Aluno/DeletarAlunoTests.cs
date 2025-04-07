using AlunoContext.Application.Commands.DeletarAluno;
using AlunoContext.Testes.Shared.Builders;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Testes.Unit.Aluno;

public class DeletarAlunoTests
{
    private readonly RepositorioFake _repositorioFake;
    private readonly UsuarioContextoFake _usuarioFake;
    private readonly DeletarAlunoHandler _handler;

    public DeletarAlunoTests()
    {
        _repositorioFake = new RepositorioFake();
        _usuarioFake = new UsuarioContextoFake();
        _handler = new DeletarAlunoHandler(_repositorioFake, _usuarioFake);
    }

    [Fact(DisplayName = "Deve remover aluno quando o ID for válido")]
    public void DeveRemoverAluno_Existente()
    {
        var aluno = AlunoBuilder.Novo().Construir();
        _repositorioFake.Adicionar(aluno);

        var comando = new DeletarAlunoComando(aluno.Id);
        var resultado = _handler.Handle(comando);

        Assert.True(resultado.Sucesso);
        Assert.Empty(_repositorioFake.Alunos);
    }

    [Fact(DisplayName = "Não deve remover aluno quando o ID não existir")]
    public void NaoDeveRemoverAluno_Inexistente()
    {
        var comando = new DeletarAlunoComando(Guid.NewGuid());
        var resultado = _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("não encontrado", resultado.Mensagem.ToLower());
    }
}
