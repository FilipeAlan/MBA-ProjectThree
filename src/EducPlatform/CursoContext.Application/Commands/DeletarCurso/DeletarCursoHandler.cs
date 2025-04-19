using CursoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Results;

namespace CursoContext.Application.Commands.DeletarCurso;

public class DeletarCursoHandler
{
    private readonly ICursoRepository _repositorio;
    private readonly IUnitOfWork _unitOfWork;

    public DeletarCursoHandler(
        ICursoRepository repositorio,
        IUnitOfWork unitOfWork)
    {
        _repositorio = repositorio;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletarCursoComando comando)
    {
        var curso = await _repositorio.ObterPorId(comando.Id);
        if (curso is null)
            return Result.Fail("Curso não encontrado.");

        await _repositorio.Excluir(curso);
        await _unitOfWork.Commit();

        return Result.Ok("Curso removido com sucesso.");
    }
}
