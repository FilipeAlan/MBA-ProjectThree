using BuildingBlocks.Events;
using BuildingBlocks.Messagings;
using BuildingBlocks.Results;
using MediatR;
using PagamentoContext.Domain.Aggregates;
using PagamentoContext.Domain.Enums;
using PagamentoContext.Domain.Repositories;
using PagamentoContext.Domain.ValueObjects;
using PagamentoContext.Infrastructure.Context;

namespace PagamentoContext.Applicarion.Commands
{
    public class RealizarPagamentoHandler : IRequestHandler<RealizarPagamentoComando, Result>
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IPagamentoUnityOfWork _unitOfWork;
        private readonly IMensagemBus _mensagemBus;

        public RealizarPagamentoHandler(
            IPagamentoRepository pagamentoRepository,
            IPagamentoUnityOfWork unitOfWork,
            IMensagemBus mensagemBus)
        {
            _pagamentoRepository = pagamentoRepository;
            _unitOfWork = unitOfWork;
            _mensagemBus = mensagemBus;
        }

        public async Task<Result> Handle(RealizarPagamentoComando comando, CancellationToken cancellationToken)
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

            if (pagamento.Status == StatusPagamento.Pago)
            {
                var evento = new PagamentoConfirmadoEvent(comando.MatriculaId);
                await _mensagemBus.Publicar(evento, "pagamento-confirmado");
            }

            return Result.Ok("Pagamento processado com sucesso.");
        }
    }
}
