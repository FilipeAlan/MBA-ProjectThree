using BuildingBlocks.Common;
using BuildingBlocks.Results;
using CursoContext.Domain.Aggregates;
using CursoContext.Domain.Repositories;
using CursoContext.Domain.ValueObjects;
using CursoContext.Infrastructure.Context;
using MediatR;

namespace CursoContext.Application.Commands.CadastrarCurso;

public class CadastrarCursoHandler : IRequestHandler<CadastrarCursoComando, ResultGeneric<Guid>>
{
    private readonly ICursoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly ICursoUnityOfWork _unitOfWork;

    public CadastrarCursoHandler(ICursoRepository repositorio, IUsuarioContexto usuarioContexto, ICursoUnityOfWork unitOfWork)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultGeneric<Guid>> Handle(CadastrarCursoComando comando, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(comando.Nome))
            return ResultGeneric<Guid>.Fail("O nome do curso é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.Descricao))
            return ResultGeneric<Guid>.Fail("A descrição do curso é obrigatória.");

        var conteudo = new ConteudoProgramatico(comando.Descricao, string.Empty);
        var curso = new Curso(comando.Nome, conteudo, _usuarioContexto.ObterUsuario());

        await _repositorio.Adicionar(curso);
        await _unitOfWork.Commit();

        return ResultGeneric<Guid>.Ok(curso.Id,"Curso cadastrado com sucesso.");
    }
}
