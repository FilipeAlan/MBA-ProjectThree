using AlunoContext.Application.Dto;
using AlunoContext.Domain.Repositories;

namespace AlunoContext.Application.Queries.ObterAluno;

public class ObterAlunoHandler
{
    private readonly IAlunoRepository _repositorio;

    public ObterAlunoHandler(IAlunoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<AlunoDetalheDto?> Handle(ObterAlunoQuery query)
    {
        var aluno = await _repositorio.ObterPorId(query.Id);
        if (aluno is null)
            return null;

        return new AlunoDetalheDto(aluno.Id, aluno.Nome, aluno.Email);
    }
}
