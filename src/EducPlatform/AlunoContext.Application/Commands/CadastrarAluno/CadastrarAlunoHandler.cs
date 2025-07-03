using AlunoContext.Domain.Aggregates;
using AlunoContext.Domain.Repositories;
using AlunoContext.Infrastructure.Context;
using BuildingBlocks.Common;
using BuildingBlocks.Results;
using MediatR;

namespace AlunoContext.Application.Commands.CadastrarAluno;

public class CadastrarAlunoHandler : IRequestHandler<CadastrarAlunoComando, ResultGeneric<Guid>>
{
    private readonly IAlunoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly IAlunoUnitOfWork _unitOfWork;

    public CadastrarAlunoHandler(IAlunoRepository repositorio, IUsuarioContexto usuarioContexto, IAlunoUnitOfWork unitOfWork)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultGeneric<Guid>> Handle(CadastrarAlunoComando comando, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(comando.Nome))
            return ResultGeneric<Guid>.Fail("O nome do aluno é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.Email) || !comando.Email.Contains('@'))
            return ResultGeneric<Guid>.Fail("O e-mail informado é inválido.");

        var aluno = new Aluno(comando.UsuarioId, comando.Nome, comando.Email, _usuarioContexto.ObterUsuario());
        await _repositorio.Adicionar(aluno);
        await _unitOfWork.Commit();

        return ResultGeneric<Guid>.Ok(aluno.Id, "Aluno cadastrado com sucesso.");
    }
}
