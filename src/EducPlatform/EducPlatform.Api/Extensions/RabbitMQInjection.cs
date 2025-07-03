using AlunoContext.Application.Consumers;
using BuildingBlocks.Messagings;
using CursoContext.Application.Consumers;
using RabbitMQ.Client;

namespace EducPlatform.Api.Extensions
{
    public static class RabbitMQInjection
    {
        public static IServiceCollection AddRabbitMQInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra ConnectionFactory do RabbitMQ
            services.AddSingleton(sp =>
            {
                var config = configuration.GetSection("RabbitMQ");
                return new ConnectionFactory
                {
                    HostName = config["HostName"] ?? "localhost",
                    UserName = config["UserName"] ?? "guest",
                    Password = config["Password"] ?? "guest"
                };
            });

            // Serviço de envio de mensagens
            services.AddSingleton<IMensagemBus, RabbitMQMensagemBus>();

            // Consumidores
            services.AddHostedService<PagamentoConfirmadoConsumer>();
            services.AddHostedService<VerificarCursoConsumer>();

            return services;
        }
    }
}
