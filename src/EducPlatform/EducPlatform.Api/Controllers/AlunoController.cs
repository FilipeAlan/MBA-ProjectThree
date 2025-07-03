namespace EducPlatform.Api.Controllers;

using AlunoContext.Application.Commands.DeletarAluno;
using AlunoContext.Application.Commands.EditarAluno;
using AlunoContext.Application.Commands.MatricularAluno;
using AlunoContext.Application.Queries.ListarAlunos;
using AlunoContext.Application.Queries.ObterAluno;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("alunos")]
public class AlunosController : ControllerBase
{
    private readonly IMediator _mediator;

    public AlunosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult ObterMeusDados()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new { UsuarioId = userId, Email = email });
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Listar()
    {
        var resultado = await _mediator.Send(new ListarAlunosQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterAlunoQuery(id));
        return resultado is null ? NotFound() : Ok(resultado);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Editar(Guid id, EditarAlunoComando comando)
    {
        if (id != comando.Id) return BadRequest("ID do aluno não confere.");
        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var resultado = await _mediator.Send(new DeletarAlunoComando(id));
        return resultado.Sucesso ? NoContent() : BadRequest(resultado);
    }
    [HttpPost("{id}/matriculas")]
    [Authorize]
    public async Task<IActionResult> Matricular(Guid id, [FromBody] Guid cursoId)
    {
        var comando = new MatricularAlunoComando(id, cursoId);
        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
    }

}
