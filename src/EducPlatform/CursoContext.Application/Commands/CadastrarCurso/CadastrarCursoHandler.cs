using CursoContext.Domain.Aggregates;
using CursoContext.Domain.Repositories;
using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Domain.ValueObjects;
using BuildingBlocks.Results;

namespace CursoContext.Application.Handlers;

public class CadastrarCursoHandler
{
    private readonly ICursoRepository _repositorio;

    public CadastrarCursoHandler(ICursoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public Result Handle(CadastrarCursoComando comando)
    {
        if (string.IsNullOrWhiteSpace(comando.Nome))
            return Result.Fail("O nome do curso é obrigatório.");

        if (string.IsNullOrWhiteSpace(comando.Descricao))
            return Result.Fail("A descrição do curso é obrigatória.");

        var conteudo = new ConteudoProgramatico(comando.Descricao, ""); // por enquanto vazio
        var curso = new Curso(comando.Nome, conteudo, "TDD");

        _repositorio.Adicionar(curso);

        return Result.Ok("Curso cadastrado com sucesso.");
    }
}
