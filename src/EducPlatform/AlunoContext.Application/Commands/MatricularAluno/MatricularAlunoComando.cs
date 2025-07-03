using BuildingBlocks.Results;
using MediatR;

namespace AlunoContext.Application.Commands.MatricularAluno;
public class MatricularAlunoComando : IRequest<Result>
{
    public Guid AlunoId { get; }
    public Guid CursoId { get; }
    public MatricularAlunoComando(Guid alunoId, Guid cursoId)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
    }
}

