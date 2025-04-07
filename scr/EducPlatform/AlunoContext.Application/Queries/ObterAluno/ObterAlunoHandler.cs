using AlunoContext.Application.Dtos;
using AlunoContext.Application.Queries.ObterAluno;
using AlunoContext.Domain.Repositories;

public class ObterAlunoHandler
{
    private readonly IAlunoRepository _repositorio;

    public ObterAlunoHandler(IAlunoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public AlunoDetalheDto? Handle(ObterAlunoQuery query)
    {
        var aluno = _repositorio.ObterPorId(query.Id);
        if (aluno is null)
            return null;

        return new AlunoDetalheDto(aluno.Id, aluno.Nome, aluno.Email);
    }
}
