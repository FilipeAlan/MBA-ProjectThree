namespace EducPlatform.Api.Controllers;

using BuildingBlocks.Results;
using CursoContext.Application.Commands.CadastrarAula;
using CursoContext.Application.Commands.CadastrarCurso;
using CursoContext.Application.Commands.DeletarCurso;
using CursoContext.Application.Commands.EditarCurso;
using CursoContext.Application.Dto;
using CursoContext.Application.Queries.ListarCurso;
using CursoContext.Application.Queries.ObterCurso;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("cursos")]
[Authorize(Roles = "admin")]
public class CursosController : ControllerBase
{
    private readonly IMediator _mediator;

    public CursosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var resultado = await _mediator.Send(new ListarCursosQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterCursoPorIdQuery(id));
        return resultado is null ? NotFound() : Ok(resultado);
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(CadastrarCursoComando comando)
    {
        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso
            ? CreatedAtAction(nameof(ObterPorId), new { id = resultado.Dados }, resultado)
            : BadRequest(resultado);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(Guid id, EditarCursoComando comando)
    {
        if (id != comando.Id) return BadRequest("ID do curso não confere.");
        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        var resultado = await _mediator.Send(new DeletarCursoComando(id));
        return resultado.Sucesso ? NoContent() : BadRequest(resultado);
    }

    [HttpPost("{id}/aulas")]
    public async Task<IActionResult> CadastrarAula(Guid id, CadastrarAulaComando comando)
    {
        if (id != comando.CursoId) return BadRequest("ID do curso não confere.");
        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
    }
}

