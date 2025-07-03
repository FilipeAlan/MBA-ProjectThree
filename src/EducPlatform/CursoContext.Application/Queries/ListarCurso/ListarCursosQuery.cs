using CursoContext.Application.Dto;
using MediatR;

namespace CursoContext.Application.Queries.ListarCurso;

public class ListarCursosQuery : IRequest<List<CursoDto>>
{
}