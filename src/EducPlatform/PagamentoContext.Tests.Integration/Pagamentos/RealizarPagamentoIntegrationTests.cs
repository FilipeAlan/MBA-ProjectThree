using BuildingBlocks.Common;
using BuildingBlocks.Events;
using BuildingBlocks.Messagings;
using Microsoft.EntityFrameworkCore;
using Moq;
using PagamentoContext.Applicarion.Commands;
using PagamentoContext.Domain.Enums;
using PagamentoContext.Domain.Repositories;
using PagamentoContext.Infrastructure.Context;
using PagamentoContext.Infrastructure.Repositories;
using PagamentoContext.Tests.Integration.Shared;

namespace PagamentoContext.Tests.Integration.Pagamentos;

public class RealizarPagamentoIntegrationTests : IAsyncLifetime
{
    private PagamentoDbContext _pagamentoContext = null!;  
    private RealizarPagamentoHandler _handler = null!;
    private IPagamentoRepository _pagamentoRepository = null!;  
    private IUnitOfWork _unitOfWork = null!;
    
    public Task InitializeAsync()
    {
        var mensagemBusMock = new Mock<IMensagemBus>();
        mensagemBusMock
            .Setup(m => m.Publicar(It.IsAny<PagamentoConfirmadoEvent>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _pagamentoContext = PagamentoTestDbContextFactory.CriarContexto();        

        _pagamentoRepository = new PagamentoRepository(_pagamentoContext);        
        _unitOfWork = new UnitOfWork(_pagamentoContext);
        _handler = new RealizarPagamentoHandler(_pagamentoRepository,_unitOfWork, mensagemBusMock.Object);
        return Task.CompletedTask;
    }

    [Fact(DisplayName = "Deve confirmar pagamento e salvar no banco")]
    public async Task DeveConfirmarPagamento_ComSucesso()
    {
        var comando = new RealizarPagamentoComando
        {
            MatriculaId = Guid.NewGuid(), // pode ser qualquer ID
            Valor = 500m,
            NumeroCartao = "1234567890", // termina com 0 → aprovado
            NomeTitular = "Aluno Teste",
            Validade = "12/29",
            CVV = "123"
        };

        var resultado = await _handler.Handle(comando);

        Assert.True(resultado.Sucesso);

        var pagamento = await _pagamentoContext.Pagamentos.FirstOrDefaultAsync(p => p.MatriculaId == comando.MatriculaId);
        Assert.NotNull(pagamento);
        Assert.Equal(StatusPagamento.Pago, pagamento!.Status);
    }

    [Fact(DisplayName = "Deve rejeitar pagamento e salvar no banco")]
    public async Task DeveRejeitarPagamento_ComSucesso()
    {
        var comando = new RealizarPagamentoComando
        {
            MatriculaId = Guid.NewGuid(),
            Valor = 500m,
            NumeroCartao = "9876543215", // termina com 5 → rejeitado
            NomeTitular = "Outro Aluno",
            Validade = "11/30",
            CVV = "321"
        };
        var resultado = await _handler.Handle(comando);

        Assert.True(resultado.Sucesso);

        var pagamento = await _pagamentoContext.Pagamentos.FirstOrDefaultAsync(p => p.MatriculaId == comando.MatriculaId);
        Assert.NotNull(pagamento);
        Assert.Equal(StatusPagamento.Rejeitado, pagamento!.Status);
    }

    public async Task DisposeAsync()
    {
        await _pagamentoContext.Database.EnsureDeletedAsync();        
        await _pagamentoContext.DisposeAsync();        
    }

    
}
