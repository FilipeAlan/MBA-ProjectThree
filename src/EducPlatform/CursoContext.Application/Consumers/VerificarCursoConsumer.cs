using BuildingBlocks.Events;
using BuildingBlocks.Messagings;
using CursoContext.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CursoContext.Application.Consumers;

public class VerificarCursoConsumer : BackgroundService
{
    private readonly IMensagemBus _mensagemBus;
    private readonly IServiceScopeFactory _scopeFactory;

    public VerificarCursoConsumer(IMensagemBus mensagemBus, IServiceScopeFactory scopeFactory)
    {
        _mensagemBus = mensagemBus;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _mensagemBus.ResponderAsync<VerificarCursoRequestEvent, VerificarCursoResponseEvent>(
            "verificar-curso-request",
            async request =>
            {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ICursoRepository>();

                var curso = await repo.ObterPorId(request.CursoId);
                return new VerificarCursoResponseEvent(request.CursoId, curso != null);
            });

        return Task.CompletedTask;
    }
}
