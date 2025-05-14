namespace BuildingBlocks.Results;

public class ResultGeneric<T> : Result
{
    public T Dados { get; set; }

    public static ResultGeneric<T> Ok(T dados, string mensagem = "Operação realizada com sucesso.")
        => new ResultGeneric<T> { Sucesso = true, Mensagem = mensagem, Dados = dados };

    public static new ResultGeneric<T> Fail(string mensagem)
        => new ResultGeneric<T> { Sucesso = false, Mensagem = mensagem };
}
