using CursoContext.Application.Dto;
using MediatR;

namespace CursoContext.Application.Queries.ObterCurso;

public class ObterCursoPorIdQuery : IRequest<CursoDto?>
{
    public Guid Id { get; }

    public ObterCursoPorIdQuery(Guid id)
    {
        Id = id;
    }
}
