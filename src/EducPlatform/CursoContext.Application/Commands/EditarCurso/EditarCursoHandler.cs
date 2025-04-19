using CursoContext.Domain.Repositories;
using CursoContext.Domain.ValueObjects;
using BuildingBlocks.Common;
using BuildingBlocks.Results;

namespace CursoContext.Application.Commands.EditarCurso;

public class EditarCursoHandler
{
    private readonly ICursoRepository _repositorio;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioContexto _usuarioContexto;

    public EditarCursoHandler(
        ICursoRepository repositorio,
        IUnitOfWork unitOfWork,
        IUsuarioContexto usuarioContexto)
    {
        _repositorio = repositorio;
        _unitOfWork = unitOfWork;
        _usuarioContexto = usuarioContexto;
    }

    public async Task<Result> Handle(EditarCursoComando comando)
    {
        var curso = await _repositorio.ObterPorId(comando.Id);
        if (curso is null)
            return Result.Fail("Curso não encontrado.");

        if (string.IsNullOrWhiteSpace(comando.NovoNome))
            return Result.Fail("O nome do curso é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.NovaDescricao))
            return Result.Fail("A descrição do curso é obrigatória.");

        var novoConteudo = new ConteudoProgramatico(comando.NovaDescricao, curso.Conteudo.Objetivos);
        curso.Atualizar(comando.NovoNome, novoConteudo, _usuarioContexto.ObterUsuario());

        await _unitOfWork.Commit();

        return Result.Ok("Curso editado com sucesso.");
    }
}
