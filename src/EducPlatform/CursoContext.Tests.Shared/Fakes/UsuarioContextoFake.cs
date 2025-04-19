using BuildingBlocks.Common;

namespace CursoContext.Tests.Shared.Fakes;
public class UsuarioContextoFake : IUsuarioContexto
{
    public string ObterUsuario() => "Usuário Logado";
}
