using AlunoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Results;

namespace AlunoContext.Application.Commands.DeletarAluno;

public class DeletarAlunoHandler
{
    private readonly IAlunoRepository _repositorio;    
    private readonly IUnitOfWork _unitOfWork;

    public DeletarAlunoHandler(IAlunoRepository repositorio, IUnitOfWork unitOfWork)
    {
        _repositorio = repositorio;        
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletarAlunoComando comando)
    {
        var aluno = await _repositorio.ObterPorId(comando.Id);

        if (aluno == null)
            return Result.Fail("Aluno não encontrado.");

        if (!aluno.PodeSerExcluido())
            return Result.Fail("O aluno possui matrículas ou certificados e não pode ser removido.");

        await _repositorio.Excluir(aluno);
        await _unitOfWork.Commit();

        return Result.Ok("Aluno removido com sucesso.");
    }
}
