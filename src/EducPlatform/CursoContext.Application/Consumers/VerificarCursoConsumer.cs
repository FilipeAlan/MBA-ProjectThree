using BuildingBlocks.Events;
using CursoContext.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CursoContext.Application.Consumers;

public class VerificarCursoConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<VerificarCursoConsumer> _logger;
    private readonly string _fila = "verificar-curso-request";
    private readonly ConnectionFactory _factory;

    public VerificarCursoConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<VerificarCursoConsumer> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        var config = configuration.GetSection("RabbitMQ");
        _factory = new ConnectionFactory
        {
            HostName = config["HostName"] ?? "localhost",
            UserName = config["UserName"] ?? "guest",
            Password = config["Password"] ?? "guest"
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = await _factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: _fila,
            durable: false,
            exclusive: false,
            autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var request = JsonSerializer.Deserialize<VerificarCursoRequestEvent>(Encoding.UTF8.GetString(body));

                if (request == null)
                {
                    _logger.LogWarning("Mensagem inválida recebida.");
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ICursoRepository>();
                var curso = await repo.ObterPorId(request.CursoId);

                var response = new VerificarCursoResponseEvent(request.CursoId, curso != null);

                var responseChannel = await connection.CreateChannelAsync();
                var responseBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

                await responseChannel.QueueDeclareAsync(
                    queue: "verificar-curso-response",
                    durable: false,
                    exclusive: false,
                    autoDelete: false);

                await responseChannel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "verificar-curso-response",
                    body: responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar VerificarCursoRequestEvent.");
            }
        };

        await channel.BasicConsumeAsync(
            queue: _fila,
            autoAck: true,
            consumer: consumer);
    }
}
