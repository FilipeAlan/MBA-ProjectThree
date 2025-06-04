using Microsoft.AspNetCore.Mvc;
using PagamentoContext.Applicarion.Commands;
using BuildingBlocks.Results;

namespace PagamentoContext.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagamentoController : ControllerBase
{
    private readonly RealizarPagamentoHandler _realizarPagamentoHandler;

    public PagamentoController(RealizarPagamentoHandler realizarPagamentoHandler)
    {
        _realizarPagamentoHandler = realizarPagamentoHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RealizarPagamentoComando comando)
    {
        Result resultado = await _realizarPagamentoHandler.Handle(comando);

        if (resultado.Sucesso)
            return Ok(new { mensagem = resultado.Mensagem });

        return BadRequest(new { erro = resultado.Mensagem });
    }
}
