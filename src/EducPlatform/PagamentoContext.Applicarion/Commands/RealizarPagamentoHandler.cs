using BuildingBlocks.Common;
using BuildingBlocks.Results;
using PagamentoContext.Domain.Aggregates;
using PagamentoContext.Domain.Repositories;
using PagamentoContext.Domain.ValueObjects;

namespace PagamentoContext.Application.Commands;
public class RealizarPagamentoHandler
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RealizarPagamentoHandler(IPagamentoRepository pagamentoRepository, IUnitOfWork unitOfWork)
    {
        _pagamentoRepository = pagamentoRepository;
        _unitOfWork = unitOfWork;
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

        // Simulação simples: se número do cartão termina com 0-4 é pago, senão rejeitado
        if (int.TryParse(comando.NumeroCartao.Last().ToString(), out int lastDigit) && lastDigit <= 4)
            pagamento.ConfirmarPagamento();
        else
            pagamento.RejeitarPagamento();

        await _pagamentoRepository.Adicionar(pagamento);
        await _unitOfWork.Commit();

        return Result.Ok("Pagamento processado com sucesso.");
    }
}
