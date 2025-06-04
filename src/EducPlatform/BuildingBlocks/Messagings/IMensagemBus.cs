namespace BuildingBlocks.Messagings
{
    public interface IMensagemBus
    {
        Task Publicar<T>(T mensagem, string fila) where T : class;
    }
}
