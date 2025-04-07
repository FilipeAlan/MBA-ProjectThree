namespace AlunoContext.Application.Commands.CadastrarAluno;

public class CadastrarAlunoComando
{
    public string Nome { get; }
    public string Email { get; }

    public CadastrarAlunoComando(string nome, string email)
    {
        Nome = nome;
        Email = email;
    }
}
