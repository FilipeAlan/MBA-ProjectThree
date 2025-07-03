using AlunoContext.Domain.Repositories;
using AlunoContext.Infrastructure.Context;
using BuildingBlocks.Common;
using BuildingBlocks.Events;
using Microsoft.Extensions.Configuration;
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
        private readonly ConnectionFactory _factory;
        private readonly string _fila;

        public PagamentoConfirmadoConsumer(
            IServiceScopeFactory scopeFactory,
            ILogger<PagamentoConfirmadoConsumer> logger,
            ConnectionFactory factory,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _factory = factory;

            var config = configuration.GetSection("RabbitMQ");
            _fila = config["Queue"] ?? "pagamento-confirmado";
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
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IAlunoUnitOfWork>();

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
        public async Task TestarProcessamentoDireto(byte[] body)
        {
            try
            {
                var evento = JsonSerializer.Deserialize<PagamentoConfirmadoEvent>(Encoding.UTF8.GetString(body));
                if (evento == null)
                    return;

                using var scope = _scopeFactory.CreateScope();
                var alunoRepo = scope.ServiceProvider.GetRequiredService<IAlunoRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var aluno = await alunoRepo.ObterAlunoPorMatriculaId(evento.MatriculaId);
                if (aluno == null)
                {
                    _logger.LogWarning($"Aluno com matrícula {evento.MatriculaId} não encontrado.");
                    return;
                }

                var matricula = aluno.Matriculas.FirstOrDefault(m => m.Id == evento.MatriculaId);
                if (matricula == null)
                {
                    _logger.LogWarning($"Matrícula {evento.MatriculaId} não encontrada no aluno.");
                    return;
                }

                matricula.ConfirmarPagamento("Pagamento confirmado teste");
                await alunoRepo.Atualizar(aluno);
                await unitOfWork.Commit();

                _logger.LogInformation($"Matrícula {evento.MatriculaId} ativada para aluno {aluno.Id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar pagamento confirmado.");
            }
        }

    }
}
