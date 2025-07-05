using BuildingBlocks.Events;
using BuildingBlocks.Messagings;

namespace AlunoContext.Tests.Shared.Fakes;

public class MensagemBusFake : IMensagemBus
{
    public bool cursoExistente { get; set; } = true;
    public Task Publicar<T>(T mensagem, string fila) where T : class
    {
        // não usado nos testes atuais
        return Task.CompletedTask;
    }

    public Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest mensagem, string fila, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class
    {
        if (mensagem is VerificarCursoRequestEvent req)
        {
            var cursoExiste = cursoExistente; // Simula que o curso existe
            TResponse response = new VerificarCursoResponseEvent(req.CursoId, cursoExiste) as TResponse;
            return Task.FromResult(response!);
        }

        return Task.FromResult<TResponse>(null!);
    }

    public void ResponderAsync<TRequest, TResponse>(string fila, Func<TRequest, Task<TResponse>> callback)
        where TRequest : class
        where TResponse : class
    {
        throw new NotImplementedException();//Para tenste não precisa implementação
    }
}
