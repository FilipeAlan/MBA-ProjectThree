using AlunoContext.Application.Dto;
using MediatR;

namespace AlunoContext.Application.Queries.ListarAlunos;

public class ListarAlunosQuery : IRequest<List<AlunoDto>> { }
