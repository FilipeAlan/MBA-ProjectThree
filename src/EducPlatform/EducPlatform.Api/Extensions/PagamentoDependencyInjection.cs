using BuildingBlocks.Messagings;
using Microsoft.EntityFrameworkCore;
using PagamentoContext.Applicarion.Commands;
using PagamentoContext.Domain.Repositories;
using PagamentoContext.Infrastructure.Context;
using PagamentoContext.Infrastructure.Repositories;
using RabbitMQ.Client;
using PagamentoUnitOfWorkImpl = PagamentoContext.Infrastructure.Context.UnitOfWork;

namespace EducPlatform.Api.Extensions
{
    public static class PagamentoDependencyInjection
    {
        public static IServiceCollection AddPagamentoContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PagamentoDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoUnityOfWork, PagamentoUnitOfWorkImpl>();

            // 🟩 Registro completo do RabbitMQMensagemBus com Hostname, Username e Password
            var rabbitConfig = configuration.GetSection("RabbitMQ");
            var factory = new ConnectionFactory
            {
                HostName = rabbitConfig["HostName"] ?? "localhost",
                UserName = rabbitConfig["UserName"] ?? "guest",
                Password = rabbitConfig["Password"] ?? "guest"
            };

            services.AddSingleton<IMensagemBus>(new RabbitMQMensagemBus(factory));

            // 🟩 Registra os handlers do contexto de pagamento
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RealizarPagamentoHandler).Assembly));

            return services;
        }
    }
}
