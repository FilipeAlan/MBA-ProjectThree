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

            // Cria uma fila exclusiva e anônima para a resposta
            var replyQueueDeclare = await channel.QueueDeclareAsync("", exclusive: true);
            var replyQueueName = replyQueueDeclare.QueueName;

            var consumer = new AsyncEventingBasicConsumer(channel);
            var tcs = new TaskCompletionSource<TResponse>();
            var correlationId = Guid.NewGuid().ToString();

            consumer.ReceivedAsync += async (_, ea) =>
            {
                if (ea.BasicProperties?.CorrelationId == correlationId)
                {
                    var body = ea.Body.ToArray();
                    var responseEnvelope = JsonSerializer.Deserialize<RequestEnvelope<TResponse>>(Encoding.UTF8.GetString(body));
                    if (responseEnvelope?.Payload is not null)
                    {
                        tcs.TrySetResult(responseEnvelope.Payload);
                    }
                }

                await Task.Yield();
            };

            await channel.BasicConsumeAsync(replyQueueName, autoAck: true, consumer: consumer);

            var envelope = new RequestEnvelope<TRequest>
            {
                CorrelationId = correlationId,
                Payload = mensagem
            };

            var json = JsonSerializer.Serialize(envelope);
            var bodyBytes = Encoding.UTF8.GetBytes(json);

            // Define as propriedades da mensagem
            var props = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = replyQueueName
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: fila,
                mandatory: false,
                basicProperties: props,
                body: new ReadOnlyMemory<byte>(bodyBytes));

            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(10));
            await using (timeoutCts.Token.Register(() => tcs.TrySetCanceled()))
            {
                return await tcs.Task;
            }
        }
        public void ResponderAsync<TRequest, TResponse>(string fila, Func<TRequest, Task<TResponse>> callback)
    where TRequest : class
    where TResponse : class
        {
            _ = Task.Run(async () =>
            {
                var connection = await _factory.CreateConnectionAsync();
                var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(fila, durable: false, exclusive: false, autoDelete: false);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var envelope = JsonSerializer.Deserialize<RequestEnvelope<TRequest>>(
                        Encoding.UTF8.GetString(ea.Body.ToArray()));
                    if (envelope == null) return;

                    var resposta = await callback(envelope.Payload!);
                    var responseEnvelope = new RequestEnvelope<TResponse>
                    {
                        CorrelationId = envelope.CorrelationId,
                        Payload = resposta
                    };
                    var responseJson = JsonSerializer.Serialize(responseEnvelope);
                    var responseBody = Encoding.UTF8.GetBytes(responseJson);

                    var props = new BasicProperties
                    {
                        CorrelationId = envelope.CorrelationId
                    };

                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: ea.BasicProperties.ReplyTo,
                        mandatory: false,
                        basicProperties: props,
                        body: new ReadOnlyMemory<byte>(responseBody));
                };

                await channel.BasicConsumeAsync(fila, autoAck: true, consumer: consumer);
            });
        }

    }
}
