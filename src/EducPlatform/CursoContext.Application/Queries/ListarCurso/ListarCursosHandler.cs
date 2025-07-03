using CursoContext.Application.Dto;
using CursoContext.Domain.Repositories;
using MediatR;

namespace CursoContext.Application.Queries.ListarCurso
{
    public class ListarCursosHandler : IRequestHandler<ListarCursosQuery, List<CursoDto>>
    {
        private readonly ICursoRepository _repositorio;

        public ListarCursosHandler(ICursoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<CursoDto>> Handle(ListarCursosQuery request, CancellationToken cancellationToken)
        {
            var cursos = await _repositorio.Listar();
            return cursos.Select(c => new CursoDto(c.Id, c.Nome, c.Conteudo.Descricao)).ToList();
        }
    }
}
