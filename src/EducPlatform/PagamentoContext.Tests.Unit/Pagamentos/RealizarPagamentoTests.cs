using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Entities;
using PagamentoContext.Applicarion.Commands;
using PagamentoContext.Domain.Enums;
using PagamentoContext.Tests.Shared.Builders;
using PagamentoContext.Tests.Shared.Fakes;
using PagamentoContext.Tests.Unit.Fakes;

namespace PagamentoContext.Tests.Unit.Pagamentos;

public class RealizarPagamentoTests
{
    private readonly PagamentoRepositoryFake _pagamentoRepositoryFake;
    private readonly AlunoRepositoryPagamentoFake _alunoRepositoryFake;
    private readonly UnitOfWorkFake _unitOfWorkFake;
    private readonly RealizarPagamentoHandler _handler;

    public RealizarPagamentoTests()
    {
        _pagamentoRepositoryFake = new PagamentoRepositoryFake();
        _alunoRepositoryFake = new AlunoRepositoryPagamentoFake();
        _unitOfWorkFake = new UnitOfWorkFake();
        _handler = new RealizarPagamentoHandler(_pagamentoRepositoryFake, _alunoRepositoryFake, _unitOfWorkFake);
    }

    [Fact(DisplayName = "Deve confirmar pagamento e ativar matrícula com dados válidos")]
    public async Task DeveConfirmarPagamentoEAtivarMatricula_ComDadosValidos()
    {
        // Arrange
        var cursoId = Guid.NewGuid();
        

        var aluno = new Aluno("Aluno Teste", "teste@email.com", "system");
        var matricula = new MatriculaBuilder()     
     .ComCurso(cursoId)
     .Construir();

        var matriculaId = matricula.Id;

        aluno.AdicionarMatricula(matricula);
        _alunoRepositoryFake.AdicionarAluno(aluno);

        var comando = new RealizarPagamentoComando
        {
            MatriculaId = matriculaId,
            Valor = 500m,
            NumeroCartao = "1234567890", // termina com 0
            NomeTitular = "Aluno Teste",
            Validade = "12/29",
            CVV = "123"
        };


        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso);
        
        Assert.Contains(_pagamentoRepositoryFake.Pagamentos, p =>
            p.MatriculaId == matriculaId && p.Status == StatusPagamento.Pago);
        
        var alunoAtualizado = await _alunoRepositoryFake.ObterAlunoPorMatriculaId(matriculaId);
        var matriculaAtualizada = alunoAtualizado?.Matriculas.FirstOrDefault(m => m.Id == matriculaId);

        Assert.NotNull(matriculaAtualizada);
        Assert.Equal(AlunoContext.Domain.Enums.StatusMatricula.Ativa, matriculaAtualizada!.Status);
    }

    [Fact(DisplayName = "Não deve ativar matrícula se pagamento for rejeitado")]
    public async Task NaoDeveAtivarMatricula_SePagamentoRejeitado()
    {
        // Arrange
        var cursoId = Guid.NewGuid();

        var aluno = new Aluno("Aluno Teste", "teste@email.com", "system");
        var matricula = new MatriculaBuilder()
            .ComCurso(cursoId)
            .Construir();

        var matriculaId = matricula.Id;

        aluno.AdicionarMatricula(matricula);
        _alunoRepositoryFake.AdicionarAluno(aluno);

        var comando = new RealizarPagamentoComando
        {
            MatriculaId = matriculaId,
            Valor = 500m,
            NumeroCartao = "1234567897", // termina com 7 => pagamento rejeitado
            NomeTitular = "Aluno Teste",
            Validade = "12/29",
            CVV = "123"
        };

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.True(resultado.Sucesso); // O pagamento ainda é processado, mas rejeitado

        Assert.Contains(_pagamentoRepositoryFake.Pagamentos, p =>
            p.MatriculaId == matriculaId && p.Status == StatusPagamento.Rejeitado);

        var alunoAtualizado = await _alunoRepositoryFake.ObterAlunoPorMatriculaId(matriculaId);
        var matriculaAtualizada = alunoAtualizado?.Matriculas.FirstOrDefault(m => m.Id == matriculaId);

        Assert.NotNull(matriculaAtualizada);
        Assert.Equal(AlunoContext.Domain.Enums.StatusMatricula.Pendente, matriculaAtualizada!.Status);
    }
    [Fact(DisplayName = "Deve falhar se aluno não encontrado pela matrícula")]
    public async Task DeveFalhar_SeAlunoNaoEncontradoPorMatricula()
    {
        // Arrange
        var matriculaId = Guid.NewGuid(); // matrícula aleatória que não existe no repositório fake

        var comando = new RealizarPagamentoComando
        {
            MatriculaId = matriculaId,
            Valor = 500m,
            NumeroCartao = "1234567890", // termina com 0 (pagamento seria aprovado)
            NomeTitular = "Aluno Teste",
            Validade = "12/29",
            CVV = "123"
        };

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("aluno não encontrado", resultado.Mensagem.ToLower());
    }
    [Fact(DisplayName = "Deve falhar se matrícula não encontrada no aluno")]
    public async Task DeveFalhar_SeMatriculaNaoEncontradaNoAluno()
    {
        // Arrange
        var cursoId = Guid.NewGuid();

        var aluno = new Aluno("Aluno Teste", "teste@email.com", "system");
        var matricula = new MatriculaBuilder()
            .ComCurso(cursoId)
            .Construir();

        aluno.AdicionarMatricula(matricula);
        _alunoRepositoryFake.AdicionarAluno(aluno);

        var matriculaIdInexistente = Guid.NewGuid(); // matrícula diferente da que o aluno tem

        var comando = new RealizarPagamentoComando
        {
            MatriculaId = matriculaIdInexistente,
            Valor = 500m,
            NumeroCartao = "1234567890", // termina com 0 (pagamento aprovado, mas matrícula errada)
            NomeTitular = "Aluno Teste",
            Validade = "12/29",
            CVV = "123"
        };

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("aluno não encontrado para a matrícula informada.", resultado.Mensagem.ToLower());
    }
    [Fact(DisplayName = "Deve falhar se dados do cartão forem inválidos")]
    public async Task DeveFalhar_SeDadosCartaoForemInvalidos()
    {
        // Arrange
        var comando = new RealizarPagamentoComando
        {
            MatriculaId = Guid.NewGuid(),
            Valor = 500m,
            NumeroCartao = "", // <- Número vazio (inválido)
            NomeTitular = "",  // <- Nome vazio (inválido)
            Validade = "",     // <- Validade vazia (inválido)
            CVV = ""           // <- CVV vazio (inválido)
        };

        // Act
        var resultado = await _handler.Handle(comando);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("dados do cartão são obrigatórios", resultado.Mensagem.ToLower());

        // Extra: garantir que nada foi adicionado no pagamento
        Assert.Empty(_pagamentoRepositoryFake.Pagamentos);
    }

}
