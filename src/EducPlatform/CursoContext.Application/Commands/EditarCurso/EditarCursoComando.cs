using MediatR;
using BuildingBlocks.Results;

namespace CursoContext.Application.Commands.EditarCurso;

public class EditarCursoComando : IRequest<Result>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public EditarCursoComando(Guid id ,string nome,string decricao)
    {
        Id = id;
        Nome = nome;
        Descricao = decricao;
    }
}
