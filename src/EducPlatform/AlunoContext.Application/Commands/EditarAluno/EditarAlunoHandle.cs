using AlunoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Results;

namespace AlunoContext.Application.Commands.EditarAluno;

public class EditarAlunoHandler
{
    private readonly IAlunoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly IUnitOfWork _unitOfWork1;

    public EditarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto,IUnitOfWork unitOfWork)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
        _unitOfWork1 = unitOfWork;
    }

    public async Task<Result> Handle(EditarAlunoComando comando)
    {
        var aluno = await _repositorio.ObterPorId(comando.Id);
        if (aluno is null)
            return Result.Fail("Aluno não encontrado.");

        aluno.AtualizarNome(comando.Nome, _usuarioContexto.ObterUsuario());
        aluno.AtualizarEmail(comando.Email, _usuarioContexto.ObterUsuario());

        await _repositorio.Atualizar(aluno);
        await _unitOfWork1.Commit();

        return Result.Ok("Aluno atualizado com sucesso.");
    }
}
