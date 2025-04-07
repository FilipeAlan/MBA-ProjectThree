using AlunoContext.Application.Common;

namespace AlunoContext.Testes.Shared.Fakes;
public class UsuarioContextoFake : IUsuarioContexto
{
    public string ObterUsuario() => "AlunoTeste";
}
