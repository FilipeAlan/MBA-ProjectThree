using CursoContext.Application.Commands.EditarCurso;
using CursoContext.Domain.Repositories;
using CursoContext.Domain.ValueObjects;
using BuildingBlocks.Results;

namespace CursoContext.Application.Handlers;

public class EditarCursoHandler
{
    private readonly ICursoRepository _repositorio;

    public EditarCursoHandler(ICursoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public Result Handle(EditarCursoComando comando)
    {
        var curso = _repositorio.ObterPorId(comando.Id);
        if (curso is null)
            return Result.Fail("Curso não encontrado.");

        if (string.IsNullOrWhiteSpace(comando.NovoNome))
            return Result.Fail("O nome do curso é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.NovaDescricao))
            return Result.Fail("A descrição do curso é obrigatória.");

        var novoConteudo = new ConteudoProgramatico(comando.NovaDescricao, curso.Conteudo.Objetivos);
        curso.Atualizar(comando.NovoNome, novoConteudo);

        return Result.Ok("Curso editado com sucesso.");
    }
}
