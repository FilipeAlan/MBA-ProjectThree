using PagamentoContext.Infrastructure.Context;

namespace PagamentoContext.Tests.Shared.Fakes;

public class UnitOfWorkFake : IPagamentoUnityOfWork
{
    public Task Commit()
    {
        return Task.CompletedTask;
    }
}
