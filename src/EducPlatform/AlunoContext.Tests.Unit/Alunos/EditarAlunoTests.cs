using AlunoContext.Application.Commands.EditarAluno;
using AlunoContext.Tests.Shared.Builders;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Tests.Unit.Alunos;

public class EditarAlunoTests
{
    private readonly AlunoRepositorioFake _repositorioFake;
    private readonly UsuarioContextoFake _usuarioFake;
    private readonly EditarAlunoHandler _handler;

    public EditarAlunoTests()
    {
        _repositorioFake = new AlunoRepositorioFake();
        _usuarioFake = new UsuarioContextoFake();
        _handler = new EditarAlunoHandler(_repositorioFake, _usuarioFake);
    }

    [Fact(DisplayName = "Deve editar aluno quando o ID for válido")]
    public async Task DeveEditarAluno_Existente()
    {
        var aluno = AlunoBuilder.Novo()
            .ComNome("Antes")
            .ComEmail("antes@email.com")
            .Construir();

        await _repositorioFake.Adicionar(aluno);

        var comando = new EditarAlunoComando(aluno.Id, "Depois", "depois@email.com");
        var resultado = await _handler.Handle(comando);

        Assert.True(resultado.Sucesso);
        var alunoEditado = await _repositorioFake.ObterPorId(aluno.Id);
        Assert.Equal("Depois", alunoEditado!.Nome);
        Assert.Equal("depois@email.com", alunoEditado.Email);
    }

    [Fact(DisplayName = "Não deve editar aluno quando o ID não existir")]
    public async Task NaoDeveEditarUsuario_Inexistente()
    {
        var comando = new EditarAlunoComando(Guid.NewGuid(), "Novo Nome", "novo@email.com");

        var resultado = await _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("não encontrado", resultado.Mensagem.ToLower());
        Assert.Empty(_repositorioFake.Alunos);
    }
}
