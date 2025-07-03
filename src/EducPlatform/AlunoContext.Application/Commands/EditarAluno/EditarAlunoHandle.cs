using AlunoContext.Domain.Repositories;
using AlunoContext.Infrastructure.Context;
using BuildingBlocks.Common;
using BuildingBlocks.Results;
using MediatR;

namespace AlunoContext.Application.Commands.EditarAluno;

public class EditarAlunoHandler : IRequestHandler<EditarAlunoComando, Result>
{
    private readonly IAlunoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly IAlunoUnitOfWork _unitOfWork;

    public EditarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto, IAlunoUnitOfWork unitOfWork)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(EditarAlunoComando comando, CancellationToken cancellationToken)
    {
        var aluno = await _repositorio.ObterPorId(comando.Id);
        if (aluno is null)
            return Result.Fail("Aluno não encontrado.");

        aluno.AtualizarNome(comando.Nome, _usuarioContexto.ObterUsuario());
        aluno.AtualizarEmail(comando.Email, _usuarioContexto.ObterUsuario());

        await _repositorio.Atualizar(aluno);
        await _unitOfWork.Commit();

        return Result.Ok("Aluno atualizado com sucesso.");
    }
}
