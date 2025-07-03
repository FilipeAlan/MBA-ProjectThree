using BuildingBlocks.Common;
using BuildingBlocks.Results;
using CursoContext.Domain.Repositories;
using CursoContext.Infrastructure.Context;
using MediatR;

namespace CursoContext.Application.Commands.DeletarCurso;

public class DeletarCursoHandler : IRequestHandler<DeletarCursoComando, Result>
{
    private readonly ICursoRepository _cursoRepository;
    private readonly ICursoUnityOfWork _unitOfWork;

    public DeletarCursoHandler(ICursoRepository cursoRepository, ICursoUnityOfWork unitOfWork)
    {
        _cursoRepository = cursoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletarCursoComando comando, CancellationToken cancellationToken)
    {
        var curso = await _cursoRepository.ObterPorId(comando.Id);

        if (curso == null)
            return Result.Fail("Curso não encontrado.");

        await _cursoRepository.Excluir(curso);
        await _unitOfWork.Commit();

        return Result.Ok("Curso removido com sucesso.");
    }
}
