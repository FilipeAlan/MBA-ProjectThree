using BuildingBlocks.Results;
using MediatR;

namespace PagamentoContext.Applicarion.Commands;
public class RealizarPagamentoComando : IRequest<Result>
{
    public Guid MatriculaId { get; set; }
    public decimal Valor { get; set; }
    public string NumeroCartao { get; set; } = string.Empty;
    public string NomeTitular { get; set; } = string.Empty;
    public string Validade { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
}

