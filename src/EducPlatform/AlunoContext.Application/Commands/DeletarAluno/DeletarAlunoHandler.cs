using AlunoContext.Domain.Common;
using AlunoContext.Domain.Repositories;
using BuildingBlocks.Results;

namespace AlunoContext.Application.Commands.DeletarAluno;

public class DeletarAlunoHandler
{
    private readonly IAlunoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;

    public DeletarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
    }

    public async Task<Result> Handle(DeletarAlunoComando comando)
    {
        var aluno = await _repositorio.ObterPorId(comando.Id);

        if (aluno == null)
            return Result.Fail("Aluno não encontrado.");

        if (!aluno.PodeSerExcluido())
            return Result.Fail("O aluno possui matrículas ou certificados e não pode ser removido.");

        await _repositorio.Excluir(aluno);

        return Result.Ok("Aluno removido com sucesso.");
    }
}
