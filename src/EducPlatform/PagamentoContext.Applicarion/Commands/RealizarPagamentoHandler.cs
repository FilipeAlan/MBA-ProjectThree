using BuildingBlocks.Common;
using BuildingBlocks.Events;
using BuildingBlocks.Messagings;
using BuildingBlocks.Results;
using PagamentoContext.Domain.Aggregates;
using PagamentoContext.Domain.Enums;
using PagamentoContext.Domain.Repositories;
using PagamentoContext.Domain.ValueObjects;

namespace PagamentoContext.Applicarion.Commands;

public class RealizarPagamentoHandler
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMensagemBus _mensagemBus;

    public RealizarPagamentoHandler(
        IPagamentoRepository pagamentoRepository,
        IUnitOfWork unitOfWork,
        IMensagemBus mensagemBus)
    {
        _pagamentoRepository = pagamentoRepository;
        _unitOfWork = unitOfWork;
        _mensagemBus = mensagemBus;
    }

    public async Task<Result> Handle(RealizarPagamentoComando comando)
    {
        if (string.IsNullOrWhiteSpace(comando.NumeroCartao) ||
            string.IsNullOrWhiteSpace(comando.NomeTitular) ||
            string.IsNullOrWhiteSpace(comando.Validade) ||
            string.IsNullOrWhiteSpace(comando.CVV))
        {
            return Result.Fail("Dados do cartão são obrigatórios.");
        }

        var dadosCartao = new DadosCartao(
            comando.NumeroCartao,
            comando.NomeTitular,
            comando.Validade,
            comando.CVV
        );

        var pagamento = new Pagamento(comando.MatriculaId, comando.Valor, dadosCartao);

        if (int.TryParse(comando.NumeroCartao.Last().ToString(), out int lastDigit) && lastDigit <= 4)
            pagamento.ConfirmarPagamento();
        else
            pagamento.RejeitarPagamento();

        await _pagamentoRepository.Adicionar(pagamento);
        await _unitOfWork.Commit();

        // Publicar evento se confirmado
        if (pagamento.Status == StatusPagamento.Pago)
        {
            var evento = new PagamentoConfirmadoEvent(comando.MatriculaId);
            await _mensagemBus.Publicar(evento, "pagamento-confirmado");
        }

        return Result.Ok("Pagamento processado com sucesso.");
    }
}
