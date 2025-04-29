using AlunoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Results;
using PagamentoContext.Domain.Aggregates;
using PagamentoContext.Domain.Enums;
using PagamentoContext.Domain.Repositories;
using PagamentoContext.Domain.ValueObjects;

namespace PagamentoContext.Application.Commands;

public class RealizarPagamentoHandler
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IAlunoRepository _alunoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RealizarPagamentoHandler(
        IPagamentoRepository pagamentoRepository,
        IAlunoRepository alunoRepository,
        IUnitOfWork unitOfWork)
    {
        _pagamentoRepository = pagamentoRepository;
        _alunoRepository = alunoRepository;
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

        if (int.TryParse(comando.NumeroCartao.Last().ToString(), out int lastDigit) && lastDigit <= 4)
            pagamento.ConfirmarPagamento();
        else
            pagamento.RejeitarPagamento();

        // Se o pagamento foi confirmado, ativa a matrícula
        if (pagamento.Status == StatusPagamento.Pago)
        {
            var aluno = await _alunoRepository.ObterAlunoPorMatriculaId(comando.MatriculaId);
            if (aluno == null)
                return Result.Fail("Aluno não encontrado para a matrícula informada.");

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == comando.MatriculaId);
            if (matricula == null)
                return Result.Fail("Matrícula não encontrada.");

            matricula.ConfirmarPagamento("Pagamento Confirmado"); // sem checar enum externo
            await _alunoRepository.Atualizar(aluno);
        }

        await _pagamentoRepository.Adicionar(pagamento);
        await _unitOfWork.Commit();

        return Result.Ok("Pagamento processado com sucesso.");
    }
}
