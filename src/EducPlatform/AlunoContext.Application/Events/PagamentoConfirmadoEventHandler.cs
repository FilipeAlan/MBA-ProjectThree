using AlunoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Events;

namespace AlunoContext.Application.Events;

public class PagamentoConfirmadoEventHandler
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PagamentoConfirmadoEventHandler(IAlunoRepository alunoRepository, IUnitOfWork unitOfWork)
    {
        _alunoRepository = alunoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(PagamentoConfirmadoEvent notification, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepository.ObterAlunoPorMatriculaId(notification.MatriculaId);
        if (aluno == null) return;

        var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == notification.MatriculaId);
        if (matricula == null) return;

        matricula.ConfirmarPagamento("Pagamento confirmado via evento");

        await _alunoRepository.Atualizar(aluno);
        await _unitOfWork.Commit();
    }
}
