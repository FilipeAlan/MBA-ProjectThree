using AlunoContext.Application.Commands.CadastrarAluno;

namespace AlunoContext.Testes.Shared.Helpers;

public static class GeradorDeComando
{
    public static CadastrarAlunoComando CriarAlunoValido()
    {
        return new CadastrarAlunoComando("AlunoTeste", "AlunoTeste@email.com");
    }

    public static CadastrarAlunoComando CriarAlunoComEmailInvalido()
    {
        return new CadastrarAlunoComando("AlunoTeste", "email-invalido");
    }

    public static CadastrarAlunoComando CriarAlunoComNomeVazio()
    {
        return new CadastrarAlunoComando("", "AlunoTeste@email.com");
    }
}
