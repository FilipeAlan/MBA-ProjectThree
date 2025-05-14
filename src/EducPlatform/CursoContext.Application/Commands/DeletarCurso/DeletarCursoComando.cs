using BuildingBlocks.Results;
using MediatR;

namespace CursoContext.Application.Commands.DeletarCurso;

public class DeletarCursoComando : IRequest<Result>
{
    public Guid Id { get; set; }

    public DeletarCursoComando(Guid id)
    {
        Id = id;
    }
}
