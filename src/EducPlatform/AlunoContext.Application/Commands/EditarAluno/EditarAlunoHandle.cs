using AlunoContext.Application.Common;
using AlunoContext.Domain.Repositories;
using BuildingBlocks.Results;

public class EditarAlunoHandler
{
    private readonly IAlunoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;

    public EditarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
    }

    public Result Handle(EditarAlunoComando comando)
    {
        var aluno = _repositorio.ObterPorId(comando.Id);
        if (aluno is null)
            return Result.Fail("Aluno não encontrado.");

        aluno.AtualizarNome(comando.Nome, _usuarioContexto.ObterUsuario());
        aluno.AtualizarEmail(comando.Email, _usuarioContexto.ObterUsuario());

        return Result.Ok("Aluno atualizado com sucesso.");
    }
}
