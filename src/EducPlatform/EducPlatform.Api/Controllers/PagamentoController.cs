using BuildingBlocks.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PagamentoContext.Applicarion.Commands;

namespace PagamentoContext.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagamentoController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagamentoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RealizarPagamentoComando comando)
    {
        Result resultado = await _mediator.Send(comando);

        if (resultado.Sucesso)
            return Ok(new { mensagem = resultado.Mensagem });

        return BadRequest(new { erro = resultado.Mensagem });
    }
}

