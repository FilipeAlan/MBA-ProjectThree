using BuildingBlocks.Common;
using BuildingBlocks.Results;
using CursoContext.Domain.Repositories;
using CursoContext.Domain.ValueObjects;
using CursoContext.Infrastructure.Context;
using MediatR;

namespace CursoContext.Application.Commands.EditarCurso;

public class EditarCursoHandler : IRequestHandler<EditarCursoComando, Result>
{
    private readonly ICursoRepository _cursoRepository;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly ICursoUnityOfWork _unitOfWork;

    public EditarCursoHandler(ICursoRepository cursoRepository, IUsuarioContexto usuarioContexto, ICursoUnityOfWork unitOfWork)
    {
        _cursoRepository = cursoRepository;
        _usuarioContexto = usuarioContexto;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(EditarCursoComando comando, CancellationToken cancellationToken)
    {
        var curso = await _cursoRepository.ObterPorId(comando.Id);

        if (curso is null)
            return Result.Fail("Curso não encontrado.");

        if (string.IsNullOrWhiteSpace(comando.Nome))
            return Result.Fail("O nome do curso é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.Descricao))
            return Result.Fail("A descrição do curso é obrigatória.");

        curso.Atualizar(comando.Nome, new ConteudoProgramatico(comando.Descricao, ""), _usuarioContexto.ObterUsuario());

        await _cursoRepository.Atualizar(curso);
        await _unitOfWork.Commit();

        return Result.Ok("Curso atualizado com sucesso.");
    }

}
