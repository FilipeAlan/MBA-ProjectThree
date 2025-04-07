namespace BuildingBlocks.Results;

public class Result
{
    public bool Sucesso { get; }
    public string Mensagem { get; }

    private Result(bool sucesso, string mensagem)
    {
        Sucesso = sucesso;
        Mensagem = mensagem;
    }

    public static Result Ok(string mensagem = "") => new(true, mensagem);
    public static Result Fail(string mensagem) => new(false, mensagem);
}
