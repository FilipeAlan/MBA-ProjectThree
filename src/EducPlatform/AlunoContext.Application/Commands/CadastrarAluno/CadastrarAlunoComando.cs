using BuildingBlocks.Results;
using MediatR;

namespace AlunoContext.Application.Commands.CadastrarAluno;

public class CadastrarAlunoComando : IRequest<ResultGeneric<Guid>>
{
    public string Nome { get; }
    public string Email { get; }
    public Guid UsuarioId { get; }

    public CadastrarAlunoComando(Guid usuarioId, string nome, string email)
    {
        Nome = nome;
        Email = email;
        UsuarioId = usuarioId;
    }
}
