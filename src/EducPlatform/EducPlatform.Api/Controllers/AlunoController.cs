using AlunoContext.Application.Commands.CadastrarAluno;
using AlunoContext.Application.Commands.DeletarAluno;
using AlunoContext.Application.Commands.EditarAluno;
using AlunoContext.Application.Dto;
using AlunoContext.Application.Queries.ListarAlunos;
using AlunoContext.Application.Queries.ObterAluno;
using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducPlatform.Api.Controllers;

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

        return Ok(new
        {
            UsuarioId = userId,
            Email = email
        });
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ListarAlunosDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar()
    {
        var resultado = await _mediator.Send(new ListarAlunosQuery());
        return Ok(resultado);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ObterAlunoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var resultado = await _mediator.Send(new ObterAlunoQuery(id));
        return resultado is null ? NotFound() : Ok(resultado);
    }

    [HttpPost("registrar-aluno")]
    [ProducesResponseType(typeof(ResultGeneric<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cadastrar(CadastrarAlunoComando comando)
    {
        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso
            ? CreatedAtAction(nameof(ObterPorId), new { id = resultado.Dados }, resultado)
            : BadRequest(resultado);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Editar(Guid id, EditarAlunoComando comando)
    {
        if (id != comando.Id)
            return BadRequest("ID do aluno não confere.");

        var resultado = await _mediator.Send(comando);
        return resultado.Sucesso ? Ok(resultado) : BadRequest(resultado);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Remover(Guid id)
    {
        var resultado = await _mediator.Send(new DeletarAlunoComando(id));
        return resultado.Sucesso ? NoContent() : BadRequest(resultado);
    }
}
