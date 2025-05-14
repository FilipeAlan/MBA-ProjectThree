using CursoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Results;
using MediatR;

namespace CursoContext.Application.Commands.DeletarCurso;

public class DeletarCursoHandler : IRequestHandler<DeletarCursoComando, Result>
{
    private readonly ICursoRepository _cursoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletarCursoHandler(ICursoRepository cursoRepository, IUnitOfWork unitOfWork)
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
