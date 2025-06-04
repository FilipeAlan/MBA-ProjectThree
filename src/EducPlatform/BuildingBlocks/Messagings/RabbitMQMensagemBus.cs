using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BuildingBlocks.Messagings
{
    public class RabbitMQMensagemBus : IMensagemBus
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQMensagemBus(string hostName)
        {
            _factory = new ConnectionFactory() { HostName = hostName };
        }

        public async Task Publicar<T>(T mensagem, string fila) where T : class
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: fila,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(mensagem));

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: fila,
                body: body);
        }
    }
}
