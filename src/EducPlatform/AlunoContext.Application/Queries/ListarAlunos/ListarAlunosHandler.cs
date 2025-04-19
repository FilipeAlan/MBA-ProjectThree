using AlunoContext.Application.Dto;
using AlunoContext.Domain.Repositories;

namespace AlunoContext.Application.Queries.ListarAlunos;

public class ListarAlunosHandler
{
    private readonly IAlunoRepository _repositorio;

    public ListarAlunosHandler(IAlunoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<AlunoDto>> Handle(ListarAlunosQuery query)
    {
        var alunos = await _repositorio.Listar();
        return alunos.Select(aluno => new AlunoDto(aluno.Id, aluno.Nome, aluno.Email)).ToList();
    }
}
