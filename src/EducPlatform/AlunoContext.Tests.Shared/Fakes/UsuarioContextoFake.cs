﻿using BuildingBlocks.Common;

namespace AlunoContext.Tests.Shared.Fakes;
public class UsuarioContextoFake : IUsuarioContexto
{
    public string ObterUsuario() => "Usuário Logado";
}
