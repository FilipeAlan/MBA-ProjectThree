using BuildingBlocks.Results;
using MediatR;

namespace CursoContext.Application.Commands.CadastrarCurso;

public class CadastrarCursoComando : IRequest<ResultGeneric<Guid>>
{
    public string Nome { get;private set; }
    public string Descricao { get;private set; }
    public CadastrarCursoComando(string nome,string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}
