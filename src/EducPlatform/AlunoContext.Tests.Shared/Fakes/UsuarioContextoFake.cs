using AlunoContext.Domain.Common;

namespace AlunoContext.Tests.Shared.Fakes;
public class UsuarioContextoFake : IUsuarioContexto
{
    public string ObterUsuario() => "AlunoTeste";
}
