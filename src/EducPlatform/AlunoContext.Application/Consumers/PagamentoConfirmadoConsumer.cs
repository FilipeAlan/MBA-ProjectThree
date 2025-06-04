using AlunoContext.Domain.Repositories;
using BuildingBlocks.Common;
using BuildingBlocks.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AlunoContext.Application.Consumers
{
    public class PagamentoConfirmadoConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PagamentoConfirmadoConsumer> _logger;
        private readonly string _fila = "pagamento-confirmado";
        private readonly ConnectionFactory _factory;

        public PagamentoConfirmadoConsumer(
            IServiceScopeFactory scopeFactory,
            ILogger<PagamentoConfirmadoConsumer> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _factory = new ConnectionFactory
            {
                HostName = "localhost" // ou use IConfiguration para parametrizar
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
                    var mensagem = JsonSerializer.Deserialize<PagamentoConfirmadoEvent>(Encoding.UTF8.GetString(body));

                    if (mensagem == null)
                    {
                        _logger.LogWarning("Mensagem nula recebida da fila.");
                        return;
                    }

                    using var scope = _scopeFactory.CreateScope();
                    var alunoRepo = scope.ServiceProvider.GetRequiredService<IAlunoRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<BuildingBlocks.Common.IUnitOfWork>();

                    var aluno = await alunoRepo.ObterAlunoPorMatriculaId(mensagem.MatriculaId);
                    if (aluno == null) return;

                    var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == mensagem.MatriculaId);
                    if (matricula == null) return;

                    matricula.ConfirmarPagamento("Pagamento confirmado via fila");
                    await alunoRepo.Atualizar(aluno);
                    await unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem de pagamento confirmado");
                }
            };

            await channel.BasicConsumeAsync(
                queue: _fila,
                autoAck: true,
                consumer: consumer);
        }
#if DEBUG
        public async Task TestarProcessamentoDireto(byte[] body)
        {
            var mensagem = JsonSerializer.Deserialize<PagamentoConfirmadoEvent>(Encoding.UTF8.GetString(body));

            if (mensagem == null) return;

            using var scope = _scopeFactory.CreateScope();
            var alunoRepo = scope.ServiceProvider.GetRequiredService<IAlunoRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var aluno = await alunoRepo.ObterAlunoPorMatriculaId(mensagem.MatriculaId);
            if (aluno == null) return;

            var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == mensagem.MatriculaId);
            if (matricula == null) return;

            matricula.ConfirmarPagamento("Pagamento confirmado via fila");
            await alunoRepo.Atualizar(aluno);
            await unitOfWork.Commit();
        }
#endif

    }
}
