using CursoContext.Application.Dto;
using CursoContext.Domain.Repositories;

namespace CursoContext.Application.Queries.ListarCurso;

public class ListarCursosHandler
{
    private readonly ICursoRepository _repositorio;

    public ListarCursosHandler(ICursoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<CursoDto>> Handle(ListarCursosQuery query)
    {
        var cursos = await _repositorio.Listar();
        return cursos.Select(c => new CursoDto(c.Id, c.Nome, c.Conteudo.Descricao)).ToList();
    }
}