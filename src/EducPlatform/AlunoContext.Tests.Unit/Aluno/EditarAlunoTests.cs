using AlunoContext.Testes.Shared.Builders;
using AlunoContext.Testes.Shared.Fakes;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Testes.Unit.Aluno;

public class EditarAlunoTests
{
    private readonly RepositorioFake _repositorioFake;
    private readonly UsuarioContextoFake _usuarioFake;
    private readonly EditarAlunoHandler _handler;

    public EditarAlunoTests()
    {
        _repositorioFake = new RepositorioFake();
        _usuarioFake = new UsuarioContextoFake();
        _handler = new EditarAlunoHandler(_repositorioFake, _usuarioFake);
    }

    [Fact(DisplayName = "Deve editar aluno quando o ID for válido")]
    public void DeveEditarAluno_Existente()
    {
        var aluno = AlunoBuilder.Novo()
            .ComNome("Antes")
            .ComEmail("antes@email.com")
            .Construir();

        _repositorioFake.Adicionar(aluno);

        var comando = new EditarAlunoComando(aluno.Id, "Depois", "depois@email.com");
        var resultado = _handler.Handle(comando);

        Assert.True(resultado.Sucesso);
        var alunoEditado = _repositorioFake.ObterPorId(aluno.Id);
        Assert.Equal("Depois", alunoEditado!.Nome);
        Assert.Equal("depois@email.com", alunoEditado.Email);
    }

    [Fact(DisplayName = "Não deve editar aluno quando o ID não existir")]
    public void NaoDeveEditarUsuario_Inexistente()
    {
        var comando = new EditarAlunoComando(Guid.NewGuid(), "Novo Nome", "novo@email.com");

        var resultado = _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("não encontrado", resultado.Mensagem.ToLower());
        Assert.Empty(_repositorioFake.Alunos);
    }
}
