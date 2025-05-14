
using BuildingBlocks.Results;
using MediatR;

namespace AlunoContext.Application.Commands.DeletarAluno;

public class DeletarAlunoComando : IRequest<Result>
{
    public Guid Id { get; }

    public DeletarAlunoComando(Guid id)
    {
        Id = id;
    }
}
