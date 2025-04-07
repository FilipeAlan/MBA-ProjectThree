using AlunoContext.Application.Queries.ListarAlunos;
using AlunoContext.Domain.Repositories;

namespace AlunoContext.Application.Queries.ListarAlunos;

public class ListarAlunosHandler
{
    private readonly IAlunoRepository _repositorio;

    public ListarAlunosHandler(IAlunoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public List<AlunoDto> Handle(ListarAlunosQuery query)
    {
        var alunos = _repositorio.Listar();
        return alunos.Select(aluno => new AlunoDto(aluno.Id, aluno.Nome, aluno.Email)).ToList();
    }
}
