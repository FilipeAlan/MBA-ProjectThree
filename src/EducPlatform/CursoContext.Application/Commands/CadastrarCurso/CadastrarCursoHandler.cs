using CursoContext.Domain.Aggregates;
using CursoContext.Domain.Repositories;
using CursoContext.Domain.ValueObjects;
using BuildingBlocks.Common;
using BuildingBlocks.Results;

namespace CursoContext.Application.Commands.CadastrarCurso;

public class CadastrarCursoHandler
{
    private readonly ICursoRepository _repositorio;
    private readonly IUsuarioContexto _usuarioContexto;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarCursoHandler(
        ICursoRepository repositorio,
        IUsuarioContexto usuarioContexto,
        IUnitOfWork unitOfWork)
    {
        _repositorio = repositorio;
        _usuarioContexto = usuarioContexto;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CadastrarCursoComando comando)
    {
        if (string.IsNullOrWhiteSpace(comando.Nome))
            return Result.Fail("O nome do curso é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.Descricao))
            return Result.Fail("A descrição do curso é obrigatória.");

        var conteudo = new ConteudoProgramatico(comando.Descricao, string.Empty);
        var curso = new Curso(comando.Nome, conteudo, _usuarioContexto.ObterUsuario());

        await _repositorio.Adicionar(curso);
        await _unitOfWork.Commit();

        return Result.Ok("Curso cadastrado com sucesso.");
    }
}
