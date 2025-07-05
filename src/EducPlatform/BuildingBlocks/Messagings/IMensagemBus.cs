namespace BuildingBlocks.Messagings
{
    public interface IMensagemBus
    {
        Task Publicar<T>(T mensagem, string fila) where T : class;
        Task<TResponse?> RequestAsync<TRequest, TResponse>(
        TRequest mensagem,
        string fila,
        CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class;
        void ResponderAsync<TRequest, TResponse>(string fila, Func<TRequest, Task<TResponse>> callback)
        where TRequest : class
        where TResponse : class;
    }
}
