using BuildingBlocks.Common;

namespace PagamentoContext.Tests.Unit.Fakes;

public class UnitOfWorkFake : IUnitOfWork
{
    public Task Commit()
    {
        return Task.CompletedTask;
    }
}
