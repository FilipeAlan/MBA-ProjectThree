using BuildingBlocks.Results;
using CursoContext.Application.Commands.CadastrarAula;
using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Commands.DeletarCurso;
using CursoContext.Application.Commands.EditarCurso;
using CursoContext.Application.Dto;
using CursoContext.Application.Queries.ListarCurso;
using CursoContext.Application.Queries.ObterCurso;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EducPlatform.Api.Controllers;

[ApiController]
[Route("cursos")]
public class CursosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CursosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ListarCursosDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var resultado = await _mediator.Send(new ListarCursosQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ObterCursoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterCursoPorIdQuery(id));
        return resultado is null ? NotFound() : Ok(resultado);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResultGeneric<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cadastrar(CadastrarCursoComando comando)
    {
        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso
            ? CreatedAtAction(nameof(ObterPorId), new { id = resultado.Dados }, resultado)
            : BadRequest(resultado);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Editar(Guid id, EditarCursoComando comando)
    {
        if (id != comando.Id)
            return BadRequest("ID do curso não confere.");

        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        var resultado = await _mediator.Send(new DeletarCursoComando(id));
        return resultado.Sucesso ? NoContent() : BadRequest(resultado);
    }

    [HttpPost("{id}/aulas")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CadastrarAula(Guid id, CadastrarAulaComando comando)
    {
        if (id != comando.CursoId)
            return BadRequest("ID do curso não confere.");

        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
    }
}
