using CursoContext.Application.Dto;
using CursoContext.Domain.Repositories;
using MediatR;

namespace CursoContext.Application.Queries.ObterCurso;

public class ObterCursoPorIdHandler : IRequestHandler<ObterCursoPorIdQuery, CursoDto?>
{
    private readonly ICursoRepository _repositorio;

    public ObterCursoPorIdHandler(ICursoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<CursoDto?> Handle(ObterCursoPorIdQuery query, CancellationToken cancellationToken)
    {
        var curso = await _repositorio.ObterPorId(query.Id);
        if (curso is null) return null;

        return new CursoDto(
            curso.Id,
            curso.Nome,
            curso.Conteudo.Descricao
        );
    }
}
