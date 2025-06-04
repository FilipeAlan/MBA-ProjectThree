using AlunoContext.Application.Commands.CadastrarAluno;

namespace AlunoContext.Tests.Shared.Helpers;

public static class GeradorDeComando
{
    public static CadastrarAlunoComando CriarAlunoValido()
    {
        return new CadastrarAlunoComando(Guid.NewGuid(), "AlunoTeste", "AlunoTeste@email.com");
    }

    public static CadastrarAlunoComando CriarAlunoComEmailInvalido()
    {
        return new CadastrarAlunoComando(Guid.NewGuid(), "AlunoTeste", "email-invalido");
    }

    public static CadastrarAlunoComando CriarAlunoComNomeVazio()
    {
        return new CadastrarAlunoComando(Guid.NewGuid(), "", "AlunoTeste@email.com");
    }
}
