using AlunoContext.Application.Commands.DeletarAluno;
using AlunoContext.Domain.Entities;
using AlunoContext.Tests.Shared.Builders;
using AlunoContext.Tests.Shared.Fakes;
using AlunoContext.Tests.Unit.Fakes;

namespace AlunoContext.Tests.Unit.Alunos;

public class DeletarAlunoTests
{
    private readonly AlunoRepositorioFake _repositorioFake;
    private readonly UsuarioContextoFake _usuarioFake;
    private readonly DeletarAlunoHandler _handler;

    public DeletarAlunoTests()
    {
        _repositorioFake = new AlunoRepositorioFake();
        _usuarioFake = new UsuarioContextoFake();
        _handler = new DeletarAlunoHandler(_repositorioFake, _usuarioFake);
    }

    [Fact(DisplayName = "Deve remover aluno quando o ID for válido")]
    public async Task DeveRemoverAluno_Existente()
    {
        var aluno = AlunoBuilder.Novo().Construir();
        await _repositorioFake.Adicionar(aluno);

        var comando = new DeletarAlunoComando(aluno.Id);
        var resultado = await _handler.Handle(comando);

        Assert.True(resultado.Sucesso);
        Assert.Empty(_repositorioFake.Alunos);
    }

    [Fact(DisplayName = "Não deve remover aluno quando o ID não existir")]
    public async Task NaoDeveRemoverAluno_Inexistente()
    {
        var comando = new DeletarAlunoComando(Guid.NewGuid());
        var resultado = await _handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Contains("não encontrado", resultado.Mensagem.ToLower());
    }

    [Fact(DisplayName = "Não deve remover aluno com matrículas ou certificados")]
    public async Task NaoDeveRemoverAluno_ComVinculos()
    {
        var aluno = AlunoBuilder.Novo().Construir();
        aluno.AdicionarMatricula(new Matricula(Guid.NewGuid(), "TDD"));
        aluno.AdicionarCertificado(new Certificado(Guid.NewGuid(), "ABC123", "TDD"));

        await _repositorioFake.Adicionar(aluno);

        var comando = new DeletarAlunoComando(aluno.Id);
        var handler = new DeletarAlunoHandler(_repositorioFake, _usuarioFake);

        var resultado = await handler.Handle(comando);

        Assert.False(resultado.Sucesso);
        Assert.Equal("O aluno possui matrículas ou certificados e não pode ser removido.", resultado.Mensagem);
        Assert.Single(_repositorioFake.Alunos); // não foi removido
    }


}
