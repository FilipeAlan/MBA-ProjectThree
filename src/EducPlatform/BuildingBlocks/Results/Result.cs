namespace BuildingBlocks.Results;

public class Result
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;

    public static Result Ok(string mensagem = "Operação realizada com sucesso.")
        => new Result { Sucesso = true, Mensagem = mensagem };

    public static Result Fail(string mensagem)
        => new Result { Sucesso = false, Mensagem = mensagem };
}
