using BuildingBlocks.Common;

namespace AlunoContext.Tests.Shared.Fakes;

public class UnitOfWorkFake : IUnitOfWork
{
    public Task Commit() => Task.CompletedTask;
}
