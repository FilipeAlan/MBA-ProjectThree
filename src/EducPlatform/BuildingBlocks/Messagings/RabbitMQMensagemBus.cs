using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BuildingBlocks.Messagings
{
    public class RabbitMQMensagemBus : IMensagemBus
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQMensagemBus(ConnectionFactory factory)
        {
            _factory = factory;
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

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest mensagem, string fila, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class
        {
            await using var connection = await _factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            var replyQueue = await channel.QueueDeclareAsync("", exclusive: true);
            var consumer = new AsyncEventingBasicConsumer(channel);

            var tcs = new TaskCompletionSource<TResponse>();
            var correlationId = Guid.NewGuid().ToString();

            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var responseEnvelope = JsonSerializer.Deserialize<RequestEnvelope<TResponse>>(Encoding.UTF8.GetString(body));
                if (responseEnvelope?.CorrelationId == correlationId)
                {
                    tcs.TrySetResult(responseEnvelope.Payload!);
                }
                await Task.Yield();
            };

            await channel.BasicConsumeAsync(replyQueue.QueueName, true, consumer);

            // Embute o correlationId dentro do JSON, pois não dá para usar basicProperties
            var envelope = new RequestEnvelope<TRequest>
            {
                CorrelationId = correlationId,
                Payload = mensagem
            };
            var json = JsonSerializer.Serialize(envelope);
            var bodyBytes = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: fila,
                body: bodyBytes);

            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(10));

            await using (timeoutCts.Token.Register(() => tcs.TrySetCanceled()))
            {
                return await tcs.Task;
            }
        }

    }

}
