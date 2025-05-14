using MediatR;
using BuildingBlocks.Results;

namespace CursoContext.Application.Commands.CadastrarAula;

public class CadastrarAulaComando : IRequest<Result>
{
    public Guid CursoId { get; private set; }
    public string Titulo { get; private set; }
    public string Conteudo { get; private set; }
    public CadastrarAulaComando(Guid id,string titulo,string conteudo)
    {
        CursoId = id;
        Titulo = titulo;
        Conteudo = conteudo;
    }
}
