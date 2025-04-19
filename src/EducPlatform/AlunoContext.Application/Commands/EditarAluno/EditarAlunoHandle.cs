using AlunoContext.Domain.Common;
using AlunoContext.Domain.Repositories;
using BuildingBlocks.Results;

namespace AlunoContext.Application.Commands.EditarAluno;

public class EditarAlunoHandler
{
    private readonly IAlunoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;

    public EditarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
    }

    public async Task<Result> Handle(EditarAlunoComando comando)
    {
        var aluno = await _repositorio.ObterPorId(comando.Id);
        if (aluno is null)
            return Result.Fail("Aluno não encontrado.");

        aluno.AtualizarNome(comando.Nome, _usuarioContexto.ObterUsuario());
        aluno.AtualizarEmail(comando.Email, _usuarioContexto.ObterUsuario());

        await _repositorio.Atualizar(aluno);

        return Result.Ok("Aluno atualizado com sucesso.");
    }
}
