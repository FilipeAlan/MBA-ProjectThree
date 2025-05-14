using BuildingBlocks.Common;

namespace PagamentoContext.Tests.Shared.Fakes;

public class UnitOfWorkFake : IUnitOfWork
{
    public Task Commit()
    {
        return Task.CompletedTask;
    }
}
