using BuildingBlocks.Events;
using BuildingBlocks.Messagings;
using Moq;
using PagamentoContext.Applicarion.Commands;
using PagamentoContext.Tests.Shared.Fakes;
using PagamentoContext.Tests.Unit.Fakes;

namespace PagamentoContext.Tests.Unit.Pagamentos;

public class RealizarPagamentoTests
{
    private readonly PagamentoRepositoryFake _pagamentoRepositoryFake;    
    private readonly UnitOfWorkFake _unitOfWorkFake;
    private readonly RealizarPagamentoHandler _handler;

    public RealizarPagamentoTests()
    {
        var mensagemBusMock = new Mock<IMensagemBus>();

        mensagemBusMock
            .Setup(m => m.Publicar(It.IsAny<PagamentoConfirmadoEvent>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _pagamentoRepositoryFake = new PagamentoRepositoryFake();        
        _unitOfWorkFake = new UnitOfWorkFake();
        _handler = new RealizarPagamentoHandler(_pagamentoRepositoryFake, _unitOfWorkFake, mensagemBusMock.Object);
    }
    
    [Fact(DisplayName = "Pagamento pode ser processado mesmo que matrícula ainda não exista (eventualmente rejeitado)")]
    public async Task PagamentoPodeSerProcessado_MesmoSemMatricula()
    {
        // Arrange
        var comando = new RealizarPagamentoComando
        {
            MatriculaId = Guid.NewGuid(), // matrícula fictícia
            Valor = 500m,
            NumeroCartao = "1234567890", // termina com 0 = pagamento aprovado
            NomeTitular = "Aluno Inexistente",
            Validade = "12/29",
            CVV = "123"
        };

        // Act
        var resultado = await _handler.Handle(comando, CancellationToken.None);

        // Assert
        Assert.True(resultado.Sucesso); // Pagamento é processado com sucesso
                                        // A validação de matrícula será feita no consumer
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
        var resultado = await _handler.Handle(comando, CancellationToken.None);

        // Assert
        Assert.False(resultado.Sucesso);
        Assert.Contains("dados do cartão são obrigatórios", resultado.Mensagem.ToLower());

        // Extra: garantir que nada foi adicionado no pagamento
        Assert.Empty(_pagamentoRepositoryFake.Pagamentos);
    }

}
