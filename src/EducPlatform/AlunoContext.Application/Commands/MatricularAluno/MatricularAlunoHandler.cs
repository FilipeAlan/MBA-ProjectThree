using AlunoContext.Domain.Entities;
using AlunoContext.Domain.Repositories;
using AlunoContext.Infrastructure.Context;
using BuildingBlocks.Common;
using BuildingBlocks.Events;
using BuildingBlocks.Messagings;
using BuildingBlocks.Results;
using MediatR;

namespace AlunoContext.Application.Commands.MatricularAluno;

public class MatricularAlunoHandler : IRequestHandler<MatricularAlunoComando, Result>
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly IMensagemBus _mensagemBus;
    private readonly IAlunoUnitOfWork _unitOfWork;

    public MatricularAlunoHandler(
        IAlunoRepository alunoRepository,
        IUsuarioContexto usuarioContexto,
        IMensagemBus mensagemBus,
        IAlunoUnitOfWork unitOfWork)
    {
        _alunoRepository = alunoRepository;
        _usuarioContexto = usuarioContexto;
        _mensagemBus = mensagemBus;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(MatricularAlunoComando comando, CancellationToken cancellationToken)
    {
        var aluno = await _alunoRepository.ObterPorId(comando.AlunoId);
        if (aluno is null)
            return Result.Fail("Aluno não encontrado.");

        // Envia mensagem de verificação para o contexto de Curso
        var requestEvent = new VerificarCursoRequestEvent(comando.CursoId);
        var response = await _mensagemBus.RequestAsync<VerificarCursoRequestEvent, VerificarCursoResponseEvent>(
            requestEvent,
            "verificar-curso-request",
            cancellationToken);

        if (response == null || !response.Existe)
            return Result.Fail("Curso não encontrado.");

        var jaMatriculado = aluno.Matriculas.Any(m => m.CursoId == comando.CursoId);
        if (jaMatriculado)
            return Result.Fail("Aluno já está matriculado nesse curso.");

        var matricula = new Matricula(comando.CursoId, _usuarioContexto.ObterUsuario());
        aluno.AdicionarMatricula(matricula);

        await _alunoRepository.Atualizar(aluno);
        await _unitOfWork.Commit();
        return Result.Ok("Aluno matriculado com sucesso.");
    }
}
